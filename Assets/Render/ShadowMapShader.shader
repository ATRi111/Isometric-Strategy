Shader "Custom/ShadowMapShader"
{
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "ShadowMapShader"
            ZWrite On
            ZTest LEqual
            Cull Off
            Blend One Zero

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:SetupProcedural

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct GroundFaceInstance
            {
                float3 origin;
                float3 uAxis;
                float3 vAxis;
            };

            StructuredBuffer<GroundFaceInstance> _FaceBuffer;
            float4x4 _CellToShadowUV;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                uint instanceID   : SV_InstanceID;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float  depth01     : TEXCOORD0;
            };

            void SetupProcedural() {}

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                GroundFaceInstance face = _FaceBuffer[IN.instanceID];
                float3 cellPos = face.origin + IN.uv.x * face.uAxis + IN.uv.y * face.vAxis;
                float4 shadowCoord = mul(_CellToShadowUV, float4(cellPos, 1.0));

                float clipX = shadowCoord.x * 2.0 - 1.0;
                float clipY = -shadowCoord.y * 2.0 + 1.0;   //纹理空间Y轴朝下，裁剪空间Y轴朝上

                #if UNITY_REVERSED_Z
                    float clipZ = 1.0 - saturate(shadowCoord.z);
                #else
                    float clipZ = lerp(UNITY_NEAR_CLIP_VALUE, 1.0, saturate(shadowCoord.z));
                #endif

                OUT.positionHCS = float4(clipX,clipY, clipZ, 1.0);
                OUT.depth01 = saturate(shadowCoord.z);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 把深度写到颜色 RT 的 R 通道，GroundShader 仍按原来方式采样 _ShadowMap.r
                return half4(IN.depth01, 0, 0, 1);
            }
            ENDHLSL
        }
    }
}
