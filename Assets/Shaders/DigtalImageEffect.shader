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
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
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
                // Depth
                fixed4 center = tex2D(_CameraDepthTexture, i.uv);
                fixed4 d_right = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(1,0) * _Thickness);
                fixed4 d_left = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(-1,0) * _Thickness);
                fixed4 d_up = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,1) * _Thickness);
                fixed4 d_down = tex2D(_CameraDepthTexture, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,-1) * _Thickness);
                
                // Normals
                //fixed4 n_center = tex2D(i.worldNormal, i.uv);
                //fixed4 n_right = tex2D(i.worldNormal, i.uv + _CameraDepthTexture_TexelSize.xy * float2(1,0) * _Thickness);
                //fixed4 n_left = tex2D(i.worldNormal, i.uv + _CameraDepthTexture_TexelSize.xy * float2(-1,0) * _Thickness);
                //fixed4 n_up = tex2D(i.worldNormal, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,1) * _Thickness);
                //fixed4 n_down = tex2D(i.worldNormal, i.uv + _CameraDepthTexture_TexelSize.xy * float2(0,-1) * _Thickness);
                
                
                float depthLines = clamp(
                (
                    (center.x - d_right.x)
                    + (center.x - d_left.x)
                    + (center.x - d_up.x)
                    + (center.x - d_down.x)) 
                    * _Scale, 0, 1
                );

                //float normalLines = clamp(
                //(
                //    (center.x - n_right.x)
                //    + (center.x - n_left.x)
                //    + (center.x - n_up.x)
                //    + (center.x - n_down.x))
                //    * _Scale, 0, 1
                //);

                //return depthLines * _Color;
                return float4(i.worldNormal, 1);
            }
            ENDCG
        }
    }
}
