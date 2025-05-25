// Upgrade NOTE: replaced '_LightMat0' with 'unity_WorldToLight'

Shader "Custom/GroundShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        
        _CellPosition("CellPosition",Vector) = (0,0,0,1)
        
        _Ambient("Ambient",Range(0, 1)) = 0.2
        _Diffuse("Diffuse",Range(0, 1)) = 0.8
        _Specular("Specular",Range(0, 1)) = 0
        _Gloss("Gloss",Range(8, 256)) = 16
        _ShadowBias("ShadowBias",Range(0, 0.01)) = 0

        _View("View", Vector) = (1,1,-1,0)
        _LightColor("LightColor", Color) = (1,1,1,1)
        _LightDirection("LightDirection",Vector) = (0,1,-2,0)
        _ShadowMap("ShadowMap", 2D) = "black" {}
        _LightMat0("LightMat0",Vector) = (1,0,0,0)
        _LightMat1("LightMat1",Vector) = (0,1,0,0) 
        _LightMat2("LightMat2",Vector) = (0,0,1,0) 
        _LightMat3("LightMat3",Vector) = (0,0,0,1) 

        _NormalMap("NormalMap", 2D) = "blue" {}

        _PawnPositionMap("PawnPositionMap",2D) = "white"{}
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
            sampler2D _NormalMap;

            float4 _CellPosition;

            float _Ambient;
            float _Diffuse;
            float _Specular;
            float _Gloss;
            float _ShadowBias;

            float4 _LightMat0;
            float4 _LightMat1;
            float4 _LightMat2;
            float4 _LightMat3;

            sampler2D _PawnPositionMap;    //必然为30×1宽度的纹理，用于记录[-100,100]范围内的坐标
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                return output;
            }

            float CalcVisibility(sampler2D shadowMap, float3 mapCoord, float bias)
            {
                float distance = mapCoord.z;
                float closestDistance = saturate(tex2D(shadowMap, mapCoord).r);

                return step(distance - bias, closestDistance);
            }

            //判断射线是否与圆柱(底面中心位于原点)相交，相交则返回1，否则返回0
            float LineSegmentCastCylinder(float height, float radius, float3 from, float3 direction)
            {
                float uIn = 0, uOut = 1;
                float u1, u2;

                //先投影到xy平面，求线段与圆的交点
                if (direction.x == 0)
                {
                    float d = radius * radius - from.x * from.x;
                    if (d < 0)
                        return 0;
                    float y1 = -sqrt(d);
                    float y2 = -y1;
                    u1 = (y1 - from.y) / direction.y;
                    u2 = (y2 - from.y) / direction.y;
                }
                else
                {
                    //y=kx+m
                    float k = direction.y / direction.x;
                    float m = from.y - k * from.x;
                    //ax^2+bx+c=0
                    float a = k * k + 1;
                    float b = 2 * m * k;
                    float c = m * m - radius * radius;
                    float d = b * b - 4 * a * c;
                    if (d < 0)
                        return 0;
                    float x1 = (-b - sqrt(d)) / 2 / a;
                    float x2 = (-b + sqrt(d)) / 2 / a;
                    u1 = (x1 - from.x) / direction.x;
                    u2 = (x2 - from.x) / direction.x;
                }

                if (u1 > u2)
                {
                    float temp = u1;
                    u1 = u2;
                    u2 = temp;
                }
                uIn = max(uIn, u1);
                uOut = min(uOut, u2);
                if (uIn >= uOut)
                    return 0;

                float z1 = from.z + uIn * direction.z;
                float z2 = from.z + uOut * direction.z;
                if (z1 >= 0 && z1 < height || z2 >= 0 && z2 < height)
                    return 1;

                return 0;
            }

            float PawnCover(float3 pawnPosition, float3 cellOffset)
            {
                return 1 - LineSegmentCastCylinder(3, 0.32, _CellPosition + cellOffset - pawnPosition, -_LightDirection);
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

            float4 CalcLight(float3 normal, float3 cellOffset)
            {
                float3 light = -normalize(_LightDirection);
                float3 view = normalize(_View);
                float4x4 lightMat = float4x4(_LightMat0,_LightMat1,_LightMat2,_LightMat3);
                float4 mapCoord = mul(lightMat, float4(_CellPosition + cellOffset, 1));
                float visibility = CalcVisibility(_ShadowMap, mapCoord, _ShadowBias);
                
                UNITY_UNROLL
                for(int i = 0; i < 30; i++)
                {
                    half2 uv = half2((i + 0.5) / 30, 0.5);
                    float3 p = tex2D(_PawnPositionMap, uv);
                    p = 200 * p - float3(100, 100, 100) + float3(0.5, 0.5, 0);
                    visibility = min(visibility, PawnCover(p, cellOffset));
                }
                return BlinnPhong(_LightColor, light, normal, view,
                    _Ambient, _Diffuse, _Specular, _Gloss, visibility);
            }

            inline float3 GetNormal(half2 uv)
            {
                float3 normal = tex2D(_NormalMap, uv);
                normal = normalize(2 * normal - float3(1, 1, 1));
                return normal;
            }
            
            float3 CalcColorUp(half2 uv)
            {
                float x = uv.x + 1.5 * uv.y - 1;
                float y = - uv.x + 1.5 * uv.y;
                float3 cellOffset = float3(x, y, 1);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(normal, cellOffset);
                return lightColor;
            }

            float3 CalcColorLeft(half2 uv)
            {
                float x = 2 * uv.x;
                float y = 2 * uv.x + 3 * uv.y - 1;
                float3 cellOffset = float3(0, 1 - x, y);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(normal, cellOffset);
                return lightColor;
            }

            float3 CalcColorRight(half2 uv)
            {
                float x = 2 * uv.x - 1;
                float y = -2 * uv.x + 3 * uv.y + 1;
                float3 cellOffset = float3(x, 0, y);
                float3 normal = GetNormal(uv);
                float4 lightColor = CalcLight(normal, cellOffset);
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
                    lightColor = CalcColorLeft(uv);
                }
                else if(uv.x > 0.5 && uv.y - 0.667 * uv.x <= 0)
                {
                    lightColor = CalcColorRight(uv);
                }
                else
                {
                    lightColor = CalcColorUp(uv);
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
