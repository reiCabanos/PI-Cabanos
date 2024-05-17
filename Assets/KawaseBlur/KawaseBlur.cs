using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class KawaseBlur : ScriptableRendererFeature
{
    [System.Serializable]
    public class KawaseBlurSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material blurMaterial = null;
        [Range(2, 15)] public int blurPasses = 1;
        [Range(1, 4)] public int downsample = 1;
        public bool copyToFramebuffer;
        public string targetName = "_KawaseBlurTexture"; // Unique name for the texture
    }

    public KawaseBlurSettings settings = new KawaseBlurSettings();
    CustomRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass("Kawase Blur");
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blurMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blur Material. Please set one in the KawaseBlur renderer profile");
            return;
        }

        m_ScriptablePass.Setup(settings);
        renderer.EnqueuePass(m_ScriptablePass);
    }

    class CustomRenderPass : ScriptableRenderPass
    {
        KawaseBlurSettings settings;

        RenderTargetHandle m_TemporaryColorTexture01;
        RenderTargetHandle m_TemporaryColorTexture02;
        string m_ProfilerTag;

        public CustomRenderPass(string profilerTag)
        {
            m_ProfilerTag = profilerTag;
            m_TemporaryColorTexture01.Init("_TemporaryColorTexture01");
            m_TemporaryColorTexture02.Init("_TemporaryColorTexture02");
        }

        public void Setup(KawaseBlurSettings settings)
        {
            this.settings = settings;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            RenderTextureDescriptor descriptor = cameraTextureDescriptor;
            descriptor.msaaSamples = 1;
            descriptor.width /= settings.downsample;
            descriptor.height /= settings.downsample;
            cmd.GetTemporaryRT(m_TemporaryColorTexture01.id, descriptor, FilterMode.Bilinear);
            cmd.GetTemporaryRT(m_TemporaryColorTexture02.id, descriptor, FilterMode.Bilinear);
            ConfigureTarget(m_TemporaryColorTexture01.Identifier());
            ConfigureTarget(m_TemporaryColorTexture02.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
            Blit(cmd, renderingData.cameraData.renderer.cameraColorTarget, m_TemporaryColorTexture01.Identifier());

            for (int i = 0; i < settings.blurPasses; i++)
            {
                cmd.SetGlobalFloat("_offset", (float)i + 1f);
                Blit(cmd, m_TemporaryColorTexture01.Identifier(), m_TemporaryColorTexture02.Identifier(), settings.blurMaterial);
                Blit(cmd, m_TemporaryColorTexture02.Identifier(), m_TemporaryColorTexture01.Identifier(), settings.blurMaterial);
            }

            if (settings.copyToFramebuffer)
            {
                Blit(cmd, m_TemporaryColorTexture01.Identifier(), renderingData.cameraData.renderer.cameraColorTarget);
            }
            else
            {
                cmd.SetGlobalTexture(settings.targetName, m_TemporaryColorTexture01.Identifier());
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(m_TemporaryColorTexture01.id);
            cmd.ReleaseTemporaryRT(m_TemporaryColorTexture02.id);
        }
    }
}
