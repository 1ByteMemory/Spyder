﻿Shader "Custom/InactiveDimension"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("XRay Edge Color", Color) = (1,0,0,0)
    }
    SubShader
    {
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
            //"RenderType" = "Opaque"
			"Switchable" = "true"
		}

        //Cull Off
    	//ZWrite Off
		//ZTest Always
		//Blend One One

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
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
