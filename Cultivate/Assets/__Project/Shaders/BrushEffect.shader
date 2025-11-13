Shader "CL/BrushEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrushPos ("BrushPos", Vector) = (0, 0, 0, 0)
        _BrushColor ("Brush Color", Color) = (1, 1, 1, 1)
        _BrushSize ("Brush Size", float) = 0.01
        // CanvasSize
        _ScaleX ("ScaleX", float) = 0.01
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        Ztest Always
        Tags { "RenderType"="Opaque" }

        Pass
        {
			Name "Normal"
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCg.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BrushPos;
            float4 _BrushColor;
            float _BrushSize;
            float _ScaleX;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 uv = i.uv;
                uv.x = uv.x * _ScaleX;
                _BrushPos.x = _BrushPos.x * _ScaleX;

                if (length(uv - _BrushPos.xy) < _BrushSize)
                {
                    col = _BrushColor;
                }
                
                return col;
            }
            ENDCG
        }

        Pass
        {
            Name "White"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCg.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
