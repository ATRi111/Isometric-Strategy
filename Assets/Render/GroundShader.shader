Shader "Custom/GroundShader" {
    Properties {
        [PerRendererData]
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap("NormalMap", 2D) = "blue" {}
        
        _CellPosition("CellPosition",Vector) = (0,0,0,1)
        
        _Ambient("Ambient",Range(0, 1)) = 0.2
        _Diffuse("Diffuse",Range(0, 1)) = 0.8
        _Specular("Specular",Range(0, 1)) = 0
        _Gloss("Gloss",Range(8, 256)) = 16
        _ShadowBias("ShadowBias",Range(0, 0.01)) = 0

        _View("View", Vector) = (1,1,-1,0)
        _LightColor("LightColor", Color) = (1,1,1,1)
        _LightDirection("LightDirection",Vector) = (0,1,-2,0)
        _ShadowMap("ShadowMap", 2D) = "white" {}
    }
    
    SubShader {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent+1" 
            "RenderPipeline"="UniversalRenderPipeline"
        }
        
        Pass {
            Name " GroundShader"
            Cull Off
            ZWrite Off 
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex Vertex_shader
            #pragma fragment Fragment_shader
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct VertexInput 
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct VertexOutput 
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            TEXTURE2D(_MainTex);        SAMPLER(sampler_MainTex);
            TEXTURE2D(_ShadowMap);      SAMPLER(sampler_ShadowMap);
            TEXTURE2D(_NormalMap);      SAMPLER(sampler_NormalMap);
            
            CBUFFER_START(Const)
                float3 _View;
                float _Ambient;
                float _Diffuse;
                float _Specular;
                float _Gloss;
                float _ShadowBias;
                float4 _CellPosition;
            CBUFFER_END

            CBUFFER_START(Light)
                float3 _LightDirection;
                float4 _LightColor;
                float4x4 _CellToShadowUV;
            CBUFFER_END
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = TransformObjectToHClip(input.position.xyz);
                output.uv = input.uv;
                return output;
            }

            float CalcVisibility(Texture2D shadowMap, float3 mapCoord, float bias)
            {
                float distance = mapCoord.z;
                float closestDistance = saturate(SAMPLE_TEXTURE2D(shadowMap, sampler_ShadowMap, mapCoord.xy).r);

                return step(distance - bias, closestDistance);
            }

            float4 BlinnPhong(float4 lightColor, float3 light, float3 normal, float3 view,
                float ambient, float diffuse, float specular, float gloss, float visibility)
            {
                float3 refl = normalize(reflect(-light, normal));
                diffuse *= visibility * saturate(dot(light, normal));
                specular *= visibility * pow(saturate(dot(refl, view)), gloss);
                float4 color = (ambient + diffuse + specular) * lightColor;
                return color;
            }

            float4 CalcLight(float3 cellPosition, float3 normal, float3 cellOffset)
            {
                float3 light = -normalize(_LightDirection);
                float3 view = normalize(_View);

                float4 mapCoord = mul(_CellToShadowUV, float4(cellPosition.xyz + cellOffset, 1));
                float visibility = CalcVisibility(_ShadowMap, mapCoord.xyz, _ShadowBias);
                
                return BlinnPhong(_LightColor, light, normal, view,
                    _Ambient, _Diffuse, _Specular, _Gloss, visibility);
            }

            inline float3 GetNormal(half2 uv)
            {
                float3 normal = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uv).rgb;
                normal = normalize(2 * normal - float3(1, 1, 1));
                return normal;
            }
            
            float3 CalcColorUp(float3 cellPosition, half2 uv)
            {
                float x = uv.x + 1.5 * uv.y - 1;
                float y = - uv.x + 1.5 * uv.y;
                float3 cellOffset = float3(x, y, 1);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(cellPosition, normal, cellOffset);
                return lightColor.xyz;
            }

            float3 CalcColorLeft(float3 cellPosition, half2 uv)
            {
                float x = 2 * uv.x;
                float y = 2 * uv.x + 3 * uv.y - 1;
                float3 cellOffset = float3(0, 1 - x, y);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(cellPosition, normal, cellOffset);
                return lightColor.xyz;
            }

            float3 CalcColorRight(float3 cellPosition, half2 uv)
            {
                float x = 2 * uv.x - 1;
                float y = -2 * uv.x + 3 * uv.y + 1;
                float3 cellOffset = float3(x, 0, y);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(cellPosition, normal, cellOffset);
                return lightColor.xyz;
            }

            float4 GammaCorrection(float4 color)
            {
                float p = 1/ 2.2;
                return float4(pow(color.x, p), pow(color.y, p), pow(color.z, p), color.w);
            }

            float4 InverseGammaCorrection(float4 color)
            {
                float p = 2.2;
                return float4(pow(color.x, p), pow(color.y, p), pow(color.z, p), color.w);
            }

            float4 Fragment_shader(VertexOutput input) : SV_Target 
            {
                float3 cellPosition = _CellPosition.xyz;
                half2 uv = input.uv;
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
                float3 lightColor;
                if(uv.x <= 0.5 && uv.y + 0.667 * uv.x <= 0.667)
                {
                    lightColor = CalcColorLeft(cellPosition, uv);
                }
                else if(uv.x > 0.5 && uv.y - 0.667 * uv.x <= 0)
                {
                    lightColor = CalcColorRight(cellPosition, uv);
                }
                else
                {
                    lightColor = CalcColorUp(cellPosition, uv);
                }
                texColor = GammaCorrection(texColor);
                float4 ret = float4(lightColor, 1) * texColor;
                ret = InverseGammaCorrection(ret);
                return ret;
            }
            ENDHLSL
        }
    }
}
