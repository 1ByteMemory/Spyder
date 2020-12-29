// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DigtalImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 scrPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
                //float depth : DEPTH;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                //o.depth = -UnityObjectToViewPos(v.vertex).z * _ProjectionParams.w;
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _CameraDepthNormalsTexture;
            float _DepthScale;
            float _DepthBias;
            float _NormalScale;
            float _NormalBias;
            float _Thickness;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                
                float dCenter;
                float3 nCenter;

                float dUp, dDown, dLeft, dRight;
                float3 nUp, nDown, nLeft, nRight;

                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy), dCenter, nCenter);
                
                // Grab noraml and depth pixels from above, below and to the side
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy + float2(0,1) * _Thickness), dUp, nUp);
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy + float2(0,-1) * _Thickness), dDown, nDown);
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy + float2(-1,0) * _Thickness), dLeft, nLeft);
                DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, i.scrPos.xy + float2(1,0) * _Thickness), dRight, nRight);

                // minus them from the center and add them all tofether to get a line
                float depthValue = (dCenter - dUp) + (dCenter - dDown) + (dCenter - dLeft) + (dCenter - dRight);
                float3 normalValue3 = (nCenter - nUp) + (nCenter - nDown) + (nCenter - nLeft) + (nCenter - nRight);

                // add normal.xyz together to merge into on channel
                float normalValue = clamp(normalValue3.x + normalValue3.y + normalValue3.x, 0,1);
                
                // multiply and pow the result to increase definition
                depthValue = clamp(depthValue * _DepthScale, 0, 1);
                depthValue = clamp(pow(depthValue, _DepthBias), 0, 1);

                normalValue = clamp(pow(normalValue * _NormalScale, _NormalBias), 0,1);

                // retrun the maximum result and multiply by a color
                return max(normalValue, depthValue) * _Color;

            }
            ENDCG
        }
    }
}
