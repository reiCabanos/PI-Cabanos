using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace OccaSoftware.GaussianBlur.Runtime
{
    public class BlurRenderFeature : ScriptableRendererFeature
    {
        class BlurSourceRenderPass : ScriptableRenderPass
        {
            private const string cullingId = "_CullingSource";
            private RenderTargetHandle blurRadiusSource;

            // RGB of radius source corresponds to tint color
            // A of radius source corresponds to blur amount
            // Correct clear value is RGBA (1, 1, 1, 0).
            private static Color blankCanvas = new Color(1, 1, 1, 0);

            private static List<ShaderTagId> shaderTagIds = new List<ShaderTagId>()
            {
                new ShaderTagId("BlurMask")
            };

            public BlurSourceRenderPass()
            {
                blurRadiusSource.Init(cullingId);
            }

            private GaussianBlur gaussianBlur;

            public void Setup(GaussianBlur gaussianBlur)
            {
                this.gaussianBlur = gaussianBlur;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor descriptor = renderingData
                    .cameraData
                    .cameraTargetDescriptor;

                descriptor.width = Mathf.Max(1, descriptor.width);
                descriptor.height = Mathf.Max(1, descriptor.height);
                descriptor.depthBufferBits = 0;
                descriptor.enableRandomWrite = true;
                descriptor.msaaSamples = 1;
                descriptor.colorFormat = RenderTextureFormat.DefaultHDR;
                RenderTextureDescriptor source = renderingData.cameraData.cameraTargetDescriptor;
                source.colorFormat = RenderTextureFormat.DefaultHDR;
                source.msaaSamples = 1;
                source.enableRandomWrite = true;
                cmd.GetTemporaryRT(blurRadiusSource.id, source);

                ConfigureTarget(
                    blurRadiusSource.Identifier(),
                    renderingData.cameraData.renderer.cameraDepthTarget
                );
                ConfigureClear(ClearFlag.Color, blankCanvas);
            }

            public override void Execute(
                ScriptableRenderContext context,
                ref RenderingData renderingData
            )
            {
                CommandBuffer cmd = CommandBufferPool.Get("Blur Radius");

                Material m = CoreUtils.CreateEngineMaterial("OccaSoftware/GaussianBlurMask");
                m.SetFloat("_BlurAmount", gaussianBlur.radius.GetValue<int>());
                m.SetTexture("_MainTex", gaussianBlur.texture.GetValue<Texture>());

                DrawingSettings drawingSettings = CreateDrawingSettings(
                    shaderTagIds,
                    ref renderingData,
                    SortingCriteria.CommonTransparent
                );
                renderingData.cameraData.camera.TryGetCullingParameters(
                    out ScriptableCullingParameters cullingParameters
                );

                CullingResults cullingResults = context.Cull(ref cullingParameters);
                FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.all);
                context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

                m.SetPass(1);
                cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, m);

                cmd.SetGlobalTexture("_CullingSource", blurRadiusSource.id);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(blurRadiusSource.id);
            }
        }

        class BlurRenderPass : ScriptableRenderPass
        {
            private const string shaderName = "BlurCompute";
            private ComputeShader shader = null;

            private int kernelX;
            private int kernelY;

            private uint threadGroupSizeX;
            private uint threadGroupSizeY;
            private RenderTargetHandle blurSource;

            private RenderTargetHandle blurTargetX;
            private RenderTargetHandle blurTargetY;

            private const string sourceId = "_BlurSource";
            private const string targetIdX = "_BlurTargetX";
            private const string targetIdY = "_BlurTargetY";

            private const string cmdBufferName = "Blur";

            private static string kernelIdBlurX = "BlurX";
            private static string kernelIdBlurY = "BlurY";

            int groupsX;
            int groupsY;

            public BlurRenderPass()
            {
                blurSource.Init(sourceId);
                blurTargetX.Init(targetIdX);
                blurTargetY.Init(targetIdY);
            }

            private GaussianBlur gaussianBlur;

            public void Setup(GaussianBlur gaussianBlur)
            {
                this.gaussianBlur = gaussianBlur;
            }

            public bool LoadComputeShader()
            {
                if (shader != null)
                    return true;

                shader = (ComputeShader)Resources.Load(shaderName);
                if (shader == null)
                    return false;

                return true;
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                RenderTextureDescriptor descriptor = renderingData
                    .cameraData
                    .cameraTargetDescriptor;

                descriptor.width = Mathf.Max(1, descriptor.width);
                descriptor.height = Mathf.Max(1, descriptor.height);
                descriptor.depthBufferBits = 0;
                descriptor.enableRandomWrite = true;
                descriptor.msaaSamples = 1;
                descriptor.colorFormat = RenderTextureFormat.DefaultHDR;
                cmd.GetTemporaryRT(blurSource.id, descriptor);
                cmd.GetTemporaryRT(blurTargetX.id, descriptor);
                cmd.GetTemporaryRT(blurTargetY.id, descriptor);

                kernelX = shader.FindKernel(kernelIdBlurX);
                kernelY = shader.FindKernel(kernelIdBlurY);

                shader.GetKernelThreadGroupSizes(
                    kernelX,
                    out threadGroupSizeX,
                    out threadGroupSizeY,
                    out _
                );
                groupsX = GetGroupCount(descriptor.width, threadGroupSizeX);
                groupsY = GetGroupCount(descriptor.height, threadGroupSizeY);
            }

            public override void Execute(
                ScriptableRenderContext context,
                ref RenderingData renderingData
            )
            {
                CommandBuffer cmd = CommandBufferPool.Get(cmdBufferName);

                RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;

                Blit(cmd, source, blurSource.Identifier());

                shader.SetInt(
                    "_IgnoreLessBlurredPixels",
                    gaussianBlur.ignoreLessBlurredPixels.GetValue<bool>() ? 1 : 0
                );

                shader.SetInt(
                    ShaderParams._ScreenSizeX,
                    renderingData.cameraData.cameraTargetDescriptor.width
                );

                shader.SetInt(
                    ShaderParams._ScreenSizeY,
                    renderingData.cameraData.cameraTargetDescriptor.height
                );

                cmd.DispatchCompute(shader, kernelX, groupsX, groupsY, 1);
                cmd.DispatchCompute(shader, kernelY, groupsX, groupsY, 1);

                Blit(cmd, blurTargetY.Identifier(), source);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                CommandBufferPool.Release(cmd);
            }

            private static class ShaderParams
            {
                public static int _Radius = Shader.PropertyToID("_RADIUS");
                public static int _ScreenSizeX = Shader.PropertyToID("_ScreenSizeX");
                public static int _ScreenSizeY = Shader.PropertyToID("_ScreenSizeY");
            }

            int GetGroupCount(int textureDimension, uint groupSize)
            {
                return Mathf.CeilToInt((textureDimension + groupSize - 1) / groupSize);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                cmd.ReleaseTemporaryRT(blurSource.id);
                cmd.ReleaseTemporaryRT(blurTargetX.id);
                cmd.ReleaseTemporaryRT(blurTargetY.id);
            }
        }

        private bool DeviceSupportsComputeShaders()
        {
            const int _COMPUTE_SHADER_LEVEL = 45;
            if (SystemInfo.graphicsShaderLevel >= _COMPUTE_SHADER_LEVEL)
                return true;

            return false;
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.activeSceneChanged += Recreate;
            UnityEditor.SceneManagement.EditorSceneManager.activeSceneChangedInEditMode += Recreate;
#endif
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += Recreate;
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.activeSceneChanged -= Recreate;
            UnityEditor.SceneManagement.EditorSceneManager.activeSceneChangedInEditMode -= Recreate;
#endif
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= Recreate;
        }

        private void Recreate(
            UnityEngine.SceneManagement.Scene current,
            UnityEngine.SceneManagement.Scene next
        )
        {
            Create();
        }

        private GaussianBlur gaussianBlur;

        /// <summary>
        /// Get the Blur component from the Volume Manager stack.
        /// </summary>
        /// <returns>If Blur component is null or inactive, returns false.</returns>
        internal bool RegisterGaussianBlurStackComponent()
        {
            gaussianBlur = VolumeManager.instance.stack.GetComponent<GaussianBlur>();
            if (gaussianBlur == null)
                return false;

            return gaussianBlur.IsActive();
        }

        private BlurRenderPass blurRenderPass;
        private BlurSourceRenderPass blurSourceRenderPass;

        public override void Create()
        {
            blurSourceRenderPass = new BlurSourceRenderPass();
            blurSourceRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;

            blurRenderPass = new BlurRenderPass();
            blurRenderPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        public override void AddRenderPasses(
            ScriptableRenderer renderer,
            ref RenderingData renderingData
        )
        {
            if (IsExcludedCameraType(renderingData.cameraData.camera.cameraType))
                return;

            if (!RegisterGaussianBlurStackComponent())
                return;

            if (!blurRenderPass.LoadComputeShader())
                return;

            blurRenderPass.Setup(gaussianBlur);
            blurSourceRenderPass.Setup(gaussianBlur);
            renderer.EnqueuePass(blurSourceRenderPass);
            renderer.EnqueuePass(blurRenderPass);
        }

        private bool IsExcludedCameraType(CameraType type)
        {
            switch (type)
            {
                case CameraType.Preview:
                    return true;
                case CameraType.Reflection:
                    return true;
                default:
                    return false;
            }
        }
    }
}
