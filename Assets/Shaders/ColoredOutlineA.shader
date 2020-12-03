// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ColoredOutlineA"
{
	Properties
	{
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
	}

	SubShader
	{

		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"Switchable" = "A"
		}

		Stencil
		{
			Ref 0
			Comp NotEqual
			Pass keep
		}

		ZWrite Off
		ZTest Always
		Blend One One

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.viewDir = normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v.vertex).xyz);
				return o;
			}

			float4 _EdgeColor;

			fixed4 frag (v2f i) : SV_Target
			{
				float NdotV = 1 - dot(i.normal, i.viewDir) * 1.5;
				return _EdgeColor;
				//return _EdgeColor * NdotV;
			}

			ENDCG
		}
	}
}
