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
            sampler _CameraDepthTexture;
            fixed4 _CameraDepthTexture_TexelSize; // Vector4(1 / width, 1 / height, width, height)
            float _Scale;
            float _Thickness;
            fixed4 _Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 center = tex2D(_CameraDepthTexture, i.uv);
                fixed4 right = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(1,0) * _Thickness);
                fixed4 left = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(-1,0) * _Thickness);
                fixed4 up = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,1) * _Thickness);
                fixed4 down = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,-1) * _Thickness);
                
                float depthLines = clamp(((center.x - right.x) + (center.x - left.x) + (center.x - up.x) + (center.x - down.x)) * _Scale, 0, 1);
                
                return depthLines * _Color;

            }
            ENDCG
        }
    }
}
