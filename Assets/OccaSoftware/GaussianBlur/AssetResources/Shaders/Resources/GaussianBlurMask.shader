Shader "OccaSoftware/GaussianBlurMask"
{
    Properties
    {
        [MainColor] _Tint("Tint", Color) = (1, 1, 1)
        _BlurAmount("Blur Amount", Float) = 0
        _MainTex("Texture", 2D) = "white" {}
    }
    
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        
        Pass
        {
            Name "BlurMask"
            Tags {"LightMode" = "BlurMask"}

            Blend DstColor Zero, One One // <src rgb, dst rgb, src alpha, dst alpha>, corresponds to <multiplicative, additive>
            Cull Off
            ZWrite Off
            ZTest LEqual
            ZClip Off
            
            HLSLPROGRAM
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #pragma multi_compile_instancing
            
            
            #pragma vertex Vert
            #pragma fragment Frag
            
            CBUFFER_START(UnityPerMaterial)    
                float3 _Tint;
                float _BlurAmount;
                
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
            CBUFFER_END
            
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 positionUV : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionHCS     : SV_POSITION;
                float2 positionUV      : TEXCOORD0;
                float4 positionNDC     : TEXCOORD1;
                float3 positionVS      : TEXCOORD2;
	            UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            
            Varyings Vert(Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(IN);
	            UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
	            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.positionUV = IN.positionUV;

                VertexPositionInputs inputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionNDC = inputs.positionNDC;
                OUT.positionHCS = inputs.positionCS;
                OUT.positionVS = inputs.positionVS;

                return OUT;
            }
            
            float4 Frag(Varyings IN) : SV_Target
            {
	            UNITY_SETUP_INSTANCE_ID(IN);
	            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                float4 tex = _MainTex.Sample(sampler_MainTex, IN.positionUV);
                return float4(_Tint * tex.rgb, _BlurAmount * tex.a);
            }
            
            ENDHLSL
        }

        Pass
        {
            Name "BlurMaskFullscreen"
            Tags {"LightMode" = "BlurMask"}
            
            Blend DstColor Zero, One One // <src rgb, dst rgb, src alpha, dst alpha>, corresponds to <multiplicative, additive>
            Cull Off
            ZWrite Off
            ZTest Always
            ZClip Off
            
            HLSLPROGRAM
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #pragma multi_compile_instancing
            
            
            #pragma vertex Vert
            #pragma fragment Frag
            
            CBUFFER_START(UnityPerMaterial)    
                float3 _Tint;
                float _BlurAmount;
                
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
            CBUFFER_END
            
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 positionUV : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct Varyings
            {
                float4 positionHCS     : SV_POSITION;
                float2 positionUV      : TEXCOORD0;
	            UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            
            Varyings Vert(Attributes IN)
            {
                Varyings OUT = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(IN);
	            UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
	            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionUV = IN.positionUV;

                OUT.positionHCS = float4(IN.positionOS.xyz, 1.0);
                #if UNITY_UV_STARTS_AT_TOP
                OUT.positionHCS.y *= -1;
                #endif

                return OUT;
            }
            
            float4 Frag(Varyings IN) : SV_Target
            {
	            UNITY_SETUP_INSTANCE_ID(IN);
	            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);
                float4 tex = _MainTex.Sample(sampler_MainTex, IN.positionUV);
                return float4(_Tint * tex.rgb, _BlurAmount * tex.a);
            }
            
            ENDHLSL
        }
    }
}
        