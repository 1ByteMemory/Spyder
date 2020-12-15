Shader "Custom/Replaceable Surface"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        
        _Color ("Color", Color) = (1,1,1,1)
        _Tint("Tint", Color) = (1,1,1,1)
        
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
		_EdgeThickness("Edge Thisckness", Float) = 1
        
		_CellSize("Cell Size", Range(0,2)) = 2
		_TimeScale("Time Scale", Range(0,2)) = 1

		_Step("Step", Range(0,1)) = 1

    }

    SubShader
    {
        Tags 
        {
            "Queue" = "Geometry+1"
            "RenderType"="Opaque"
            "Switchable"="A"
        }
        LOD 200

		Stencil
		{
			Ref 1
			Comp Always
			Pass Replace
		}
        
        CGPROGRAM


        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;


        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
