Shader "Custom/GroundShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorLeft ("Color Left", Color) = (1,1,1,1)
        _ColorRight ("Color Right", Color) = (1,1,1,1)
        _ColorUp ("Color Up", Color) = (1,1,1,1)
        _TestCover ("Test Cover",Float) = 0
        
        _AOUp ("AO Up",2D) = "white" {}
        _AOLeft ("AO Left",2D) = "white" {}
        _AORight ("AO Right",2D) = "white" {}
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

            float3 _ColorLeft;
            float3 _ColorRight;
            float3 _ColorUp;

            sampler2D _AOUp;
            sampler2D _AOLeft;
            sampler2D _AORight;

            float _TestCover;
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                return output;
            }

            float CalcCover(float4 pair, float2 direction, float2 offset)
            {
                float k = 0;
                float d = length(-pair.x * direction + offset);
                k = max(k, pair.y / d);
                d = length(-pair.z * direction + offset);
                k = max(k, pair.w / d);
                float cover = atan(k) / 1.57079632679 + _TestCover;
                return clamp(cover, 0, 1);
            }

            float AmbientOcculation(sampler2D AO, float2 offset)
            {
                //从上方开始，每次逆时针旋转45°
                float2 directions[8] = {
                    {0,1},
                    {-0.707,0.707},
                    {-1,0},
                    {-0.707,-0.707},
                    {0,-1},
                    {0.707,-0.707},
                    {1,0},
                    {0.707,0.707},
                    };
                float weights[8] = {1,0.5,1,0.5,1,0.5,1,0.5};
                float cover = 0;
                float weightSum = 0;

                UNITY_UNROLL
                for(int i = 0; i < 8; i++)
                {
                    float2 direction = directions[i];
                    float4 pair = tex2D(AO, 0.4 * direction + float2(0.5, 0.5));
                    pair *= 100;
                    cover += weights[i] * CalcCover(pair, direction, offset);
                    weightSum += weights[i];
                }
                return 1 - cover / weightSum;
            }
            
            float3 CalcColorUp(float u, float v)
            {
                float x = u + 1.5 * v - 1;
                float y = -u + 1.5 * v;
                float2 offset = float2(x - 0.5, y - 0.5);
                return AmbientOcculation(_AOUp, offset) * _ColorUp;
            }

            float3 CalcColorLeft(float u, float v)
            {
                float x = 2 * u;
                float y = 2 * u + 3 * v - 1;
                float2 offset = float2(x - 0.5, 0.5 * (y - 0.5));
                return AmbientOcculation(_AOLeft, offset) * _ColorLeft;
            }

            float3 CalcColorRight(float u, float v)
            {
                float x = 2 * u - 1;
                float y = -2 * u + 3 * v + 1;
                float2 offset = float2(x - 0.5, 0.5 * (y - 0.5));
                return AmbientOcculation(_AORight, offset) * _ColorRight;
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
                float4 ret = float4(lightColor, 1) * texColor;
                return ret;
            }
            ENDCG
        }
    }
}
