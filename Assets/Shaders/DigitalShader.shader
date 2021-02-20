Shader "Custom/DigitalShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        _SizeX ("Size X", Float) = 1
        _SizeY ("Size Y", Float) = 1

        _ThicknessX ("Thickness X", Float) = 2
        _ThicknessY ("Thickness Y", Float) = 2

        _OffsetX ("Offset X", Float) = 0
        _OffsetY ("Offset Y", Float) = 0

        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        
        _Color ("Color", Color) = (1,1,1,1)
        _BckColor("Backgrond Color", Color) = (0,0,0,0)

		_EdgeThickness("Edge Thisckness", Float) = 1
        
		_CellSize("Cell Size", Range(0,2)) = 2
		_TimeScale("Time Scale", Range(0,2)) = 1

		_Step("Step", Range(0,1)) = 1
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Geometry"
            "RenderType"="Opaque"
            "Replace" = "true"
        }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float _SizeX;
            float _SizeY;
            float _ThicknessX;
            float _ThicknessY;
            float _OffsetX;
            float _OffsetY;

            half4 _Color;
            half4 _BckColor;


            fixed4 frag (v2f i) : SV_Target
            {
                float x = 1 - saturate(round(abs(frac((i.uv.x + _OffsetX) * _SizeX) * _ThicknessX)));
                float y = 1 - saturate(round(abs(frac((i.uv.y + _OffsetY) * _SizeY) * _ThicknessY)));

                float4 squares = (1 - (x + y)) * _BckColor;
                float4 lines = (x + y) * _Color;

                return squares + lines;
            }
            ENDCG
        }
    }
}
