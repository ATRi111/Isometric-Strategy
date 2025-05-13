Shader "Custom/GroundShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorLeft ("Color Left", Color) = (1,0,0,1)
        _ColorRight ("Color Right", Color) = (0,1,0,1)
        _ColorUp ("Color Up", Color) = (0,0,1,1)

        _CoverLeftLeft ("Cover Left Left", Float) = 0.0
        _CoverLeftRight ("Cover Left Right", Float) = 0.0
        _CoverLeftUp ("Cover Left Up", Float) = 0.0
        _CoverLeftDown ("Cover Left Down", Float) = 0.0

        _CoverRightLeft ("Cover Right Left", Float) = 0.0
        _CoverRightRight ("Cover Right Right", Float) = 0.0
        _CoverRightUp ("Cover Right Up", Float) = 0.0
        _CoverRightDown ("Cover Right Down", Float) = 0.0

        _CoverUpLeft ("Cover Up Left", Float) = 0.0
        _CoverUpRight ("Cover Up Right", Float) = 0.0
        _CoverUpUp ("Cover Up Up", Float) = 0.0
        _CoverUpDown ("Cover Up Down", Float) = 0.0

        _ShadowWidth ("Shadow Width", Float) = 0.1
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
            
            float _CoverLeftLeft;
            float _CoverLeftRight;
            float _CoverLeftUp;
            float _CoverLeftDown; 

            float _CoverRightLeft;
            float _CoverRightRight;
            float _CoverRightUp;
            float _CoverRightDown; 
            
            float _CoverUpLeft;
            float _CoverUpRight;
            float _CoverUpUp;
            float _CoverUpDown;

            float _ShadowWidth; //角落处阴影宽度
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                return output;
            }

            //假设左边可能有遮挡，计算正方形内部各点的遮蔽值
            float CalcShadow(float4 coverValue, half x, half y)
            {
                y = 2 * abs(y - 0.5);
                x = min(x ,_ShadowWidth) / _ShadowWidth * 1.57 + 1.57;
                float ret = 0.5 * sin(x);
                return coverValue * ret;
            }

            float3 CalcColorLeft(half2 uv)
            {
                half x = 2 * uv.x;
                half y = 2 * uv.x + 3 * uv.y -1;
                float shadow = 0;
                shadow += CalcShadow(_CoverLeftLeft, x, y);
                shadow += CalcShadow(_CoverLeftRight, 1 - x, y);
                shadow += CalcShadow(_CoverLeftUp, 1 - y, x);
                shadow += CalcShadow(_CoverLeftDown, y, x);
                return max(1 - shadow, 0) * _ColorLeft;
            }

            
            float3 CalcColorRight(half2 uv)
            {
                half x = 2 * uv.x - 1;
                half y = -2 * uv.x + 3 * uv.y + 1;
                float shadow = 0;
                shadow += CalcShadow(_CoverRightLeft, x, y);
                shadow += CalcShadow(_CoverRightRight, 1 - x, y);
                shadow += CalcShadow(_CoverRightUp, 1 - y, x);
                shadow += CalcShadow(_CoverRightDown, y, x);
                return max(1 - shadow, 0) * _ColorRight;
            }
            
            float3 CalcColorUp(half2 uv)
            {
                half x = uv.x + 1.5 * uv.y - 1;
                half y = - uv.x + 1.5 * uv.y;
                float shadow = 0;
                shadow += CalcShadow(_CoverUpLeft, x, y);
                shadow += CalcShadow(_CoverUpRight, 1 - x, y);
                shadow += CalcShadow(_CoverUpUp, 1 - y, x);
                shadow += CalcShadow(_CoverUpDown, y, x);
                return max(1 - shadow, 0) * _ColorUp;
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
    
                // 读取纹理的颜色和Alpha值
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
                texColor = InverseGammaCorrection(texColor);
                float4 ret = float4(lightColor,1) * texColor;
                return GammaCorrection(ret);
            }
            ENDCG
        }
    }
}
