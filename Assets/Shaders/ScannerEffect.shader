// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DetailTex("Texture", 2D) = "white" {}
		_ScanDistance("Scan Distance", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			float4 _CameraWS;

			VertOut vert(VertIn v)
			{
				VertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				
				//o.uv_depth = v.uv.xy;
				o.uv_depth = ComputeScreenPos(o.vertex);

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

			sampler2D _MainTex;
			sampler2D _DetailTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance;
			float _ScanWidth;
			float _LeadSharp;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;
			float4 _HBarColor;

			// Edge variables
            sampler2D _CameraDepthNormalsTexture;
            float _DepthScale;
            float _DepthBias;
            float _NormalScale;
            float _NormalBias;
            float _Thickness;
            float _Distance;
            float _Bias;
            fixed4 _Color;


			float4 horizBars(float2 p)
			{	
				return (1 - saturate(round(abs(frac(p.x * 100) * 2)))) + (1 - saturate(round(abs(frac(p.y * 100) * 2))));
			}

			float4 horizTex(float2 p)
			{
				return tex2D(_DetailTex, float2(p.x * 30, p.y * 40));
			}

			half4 frag (VertOut i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;
				half4 scannerCol = half4(0, 0, 0, 0);

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				half4 edgeCol = col;

				
				//if (dist < _ScanDistance - _ScanWidth && linearDepth < 1)
				if (dist < _ScanDistance && dist > _ScanDistance - _ScanWidth && linearDepth < 1)
				{
					float diff = 1 - (_ScanDistance - dist) / (_ScanWidth);
					half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));
					scannerCol = lerp(_TrailColor, edge, diff) + horizBars(i.uv) * _HBarColor;
					scannerCol *= diff;
				}


				float dCenter;
				float3 nCenter;

				float dUp, dDown, dLeft, dRight;
				float3 nUp, nDown, nLeft, nRight;

				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth.xy), dCenter, nCenter);
                
				// Grab noraml and depth pixels from above, below and to the side
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth.xy + float2(0,1) * _Thickness), dUp, nUp);
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth.xy + float2(0,-1) * _Thickness), dDown, nDown);
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth.xy + float2(-1,0) * _Thickness), dLeft, nLeft);
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.uv_depth.xy + float2(1,0) * _Thickness), dRight, nRight);

				dCenter = Linear01Depth(dCenter);
				dUp = Linear01Depth(dUp);
				dDown = Linear01Depth(dDown);
				dLeft = Linear01Depth(dLeft);
				dRight = Linear01Depth(dRight);


				// minus them from the center and add them all tofether to get a line
				float depthValue = (dCenter - dUp) + (dCenter - dDown) + (dCenter - dLeft) + (dCenter - dRight);
				float3 normalValue3 = (nCenter - nUp) + (nCenter - nDown) + (nCenter - nLeft) + (nCenter - nRight);

				// add normal.xyz together to merge into on channel
				float normalValue = clamp(normalValue3.x + normalValue3.y + normalValue3.x, 0,1);
                
				
				// multiply and pow the result to increase definition
				depthValue = clamp(depthValue * _DepthScale, 0, 1);
				depthValue = clamp(pow(depthValue, _DepthBias), 0, 1);

				normalValue = clamp(pow(normalValue * _NormalScale, _NormalBias), 0,1);

				if (dist < _ScanDistance && linearDepth < 1)
				{
					edgeCol = max(normalValue, depthValue) * _Color;
					
					float diff = (_ScanDistance - dist) / (_ScanWidth);
					half4 edge = lerp(col, edgeCol, pow(diff, _LeadSharp));
					edgeCol = lerp(edgeCol, edge, diff);
					edgeCol *= diff;
				}

				return edgeCol + scannerCol;
			}
			ENDCG
		}
	}
}
