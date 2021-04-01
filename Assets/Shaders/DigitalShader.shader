Shader "Custom/DigitalShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        _Size ("Size", Float) = 1

        _Thickness ("Thickness", Range(0,1)) = 0.1


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


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };

            float _Size;
            sampler2D _MainTex;

            v2f vert (appdata_full v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldPos = worldPos;
                o.worldNormal = worldNormal;

                return o;
            }

            float _Thickness;

            half4 _Color;
            half4 _BckColor;


            fixed4 frag (v2f i) : SV_Target
            {
                //float2 xz = frac(i.worldPos.xz / _Size);
                //float2 xy = frac(i.worldPos.xy / _Size);
                //float2 yz = frac(i.worldPos.yz / _Size);

                //float checker = step(c, _Thickness);

                return float4(i.worldNormal, 0);



                //float x = 1 - saturate(round(abs(frac((i.worldPos.x + _OffsetX) * _SizeX) * _ThicknessX)));
                //float y = 1 - saturate(round(abs(frac((i.worldPos.y + _OffsetY) * _SizeY) * _ThicknessY)));
                //float z = 1 - saturate(round(abs(frac((i.worldPos.z + _OffsetX) * _SizeX) * _ThicknessX)));


                //float x = step(frac(i.worldPos.x / _Size), _Thickness);
                //float y = step(frac(i.worldPos.y / _Size), _Thickness);
                //float z = step(frac(i.worldPos.z / _Size), _Thickness);

                //float4 squares = (1 - (x + y + z)) * _BckColor;
                //float4 lines = (x + y + z) * _Color;
                
                //return squares + lines;
            }
            ENDCG
        }
    }
}
