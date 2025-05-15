Shader "Custom/GroundShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorLeft ("Color Left", Color) = (1,0,0,1)
        _ColorRight ("Color Right", Color) = (0,1,0,1)
        _ColorUp ("Color Up", Color) = (0,0,1,1)
        _Map("Map",3D) = "black" {}
        _Position ("Positon", Vector) = (0,0,0,0)
        _MaxShadow ("Max Shadow",Float) = 0.8
        _TestCover ("Test Shadow",Float) = 0
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
            float4 _Position;

            sampler3D _Map;
            float _TestCover;

            float _MaxShadow = 0.8;
            
            VertexOutput Vertex_shader(VertexInput input) 
            {
                VertexOutput output;
                output.position = UnityObjectToClipPos(input.position);
                output.uv = input.uv;
                return output;
            }

            float AmbientOcculation(float3 pointPosition, float3 normal)
            {
                return 1;
            }
            
            float3 CalcColorUp(half2 uv)
            {
                float4 p = _Position;
                return AmbientOcculation(p, float3(0,0,1)) * _ColorUp;
            }

            float3 CalcColorLeft(half2 uv)
            {
                float4 p = _Position;
                return AmbientOcculation(p, float3(-1,0,0)) * _ColorLeft;
            }

            float3 CalcColorRight(half2 uv)
            {
                float4 p = _Position;
                return AmbientOcculation(p, float3(0,-1,0)) * _ColorRight;
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
                float4 ret = float4(lightColor, 1) * texColor;
                return ret;
            }
            ENDCG
        }
    }
}
