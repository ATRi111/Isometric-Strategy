Shader "Custom/GroundShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        
        _ShadowMapCoord("ShadowMapCoord",Vector) = (0,0,0,0)
        
        _Ambient("Ambient",Range(0, 1)) = 0.2
        _Diffuse("Diffuse",Range(0, 1)) = 0.8
        _Specular("Specular",Range(0, 1)) = 0
        _Gloss("Gloss",Range(8, 256)) = 16
        _ShdowBias("ShadowBias",Float) = 0

        _View("ShadowBias", Vector) = (1,1,-1,0)
        _LightColor("LightColor", Color) = (1,1,1,1)
        _LightDirection("LightDirection",Vector) = (0,1,-2,0)
        _ShadowMap("ShadowBias", 2D) = "black" {}
    }
    
    SubShader {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1"}
        
        Pass {
            Name " SpritePass"
            Cull Off
            ZWrite Off 
            Blend SrcAlpha OneMinusSrcAlpha
            
            CGPROGRAM
            #pragma vertex Vertex_shader
            #pragma fragment Fragment_shader
            #include "UnityCG.cginc"
            
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
            
            sampler2D _MainTex;
            
            float3 _View;
            float4 _LightColor;
            float3 _LightDirection;
            sampler2D _ShadowMap;

            float3 _ShadowMapCoord;
            float3 _CellPosition;

            float _Ambient;
            float _Diffuse;
            float _Specular;
            float _Gloss;
            float _ShdowBias;
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                return output;
            }

            float CalcVisibility(sampler2D shadowMap, float3 mapCoord, float bias)
            {
                // dot(mapCoord, light) 的值必然在[-1,1]之间
                float distance = mapCoord.z;
                float closestDistance = saturate(tex2D(shadowMap, mapCoord));

                if( distance - bias <= closestDistance)
                {
                    return 1;   
                }
                else
                {
                    return 0;
                }
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

            float4 CalcLight(float3 normal)
            {
                float3 light = -normalize(_LightDirection);
                float3 view = normalize(_View);
                //TODO:坐标变换
                float3 mapCoord = float3(0,0,0);
                float visibility = CalcVisibility(_ShadowMap, mapCoord, _ShdowBias);
                return BlinnPhong(_LightColor, light, normal, view, 
                    _Ambient, _Diffuse, _Specular, _Gloss, visibility);
            }
            
            float3 CalcColorUp(float u, float v)
            {
                float x = u + 1.5 * v - 1;
                float y = -u + 1.5 * v;
                float4 lightColor = CalcLight(float3(0, 0, 1));
                return lightColor;
            }

            float3 CalcColorLeft(float u, float v)
            {
                float x = 2 * u;
                float y = 2 * u + 3 * v - 1;
                float4 lightColor = CalcLight(float3(-1, 0, 0));
                return lightColor;
            }

            float3 CalcColorRight(float u, float v)
            {
                float x = 2 * u - 1;
                float y = -2 * u + 3 * v + 1;
                float4 lightColor = CalcLight(float3(0, -1, 0));
                return lightColor;
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
                half2 uv = input.uv;
                float4 texColor = tex2D(_MainTex, uv);
                float3 lightColor;
                if(uv.x <= 0.5 && uv.y + 0.667 * uv.x <= 0.667)
                {
                    lightColor = CalcColorLeft(uv.x, uv.y);
                }
                else if(uv.x > 0.5 && uv.y - 0.667 * uv.x <= 0)
                {
                    lightColor = CalcColorRight(uv.x, uv.y);
                }
                else
                {
                    lightColor = CalcColorUp(uv.x, uv.y);
                }
                texColor = GammaCorrection(texColor);
                float4 ret = float4(lightColor, 1) * texColor;
                ret = InverseGammaCorrection(ret);
                return ret;
            }
            ENDCG
        }
    }
}
