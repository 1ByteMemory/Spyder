Shader "Custom/Fragmented Shader"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white" {}
		_Tint("Tint", Color) = (1,1,1,1)

		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeThickness("Edge Thisckness", Float) = 1

		_CellSize("Cell Size", Range(0,2)) = 2
		_TimeScale("Time Scale", Range(0,2)) = 1

		_Step("Step", Range(0,1)) = 1
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"Switchable" = "A"
		}
		LOD 200

		Stencil
		{
			Ref 0
			Comp Always
		}

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		
		//--------------------------//
		//  Shattered Noise Map     //
		//--------------------------//

		CGINCLUDE

		#pragma target 3.0
		#pragma surface surf Standard alpha

		#include "RandomGenerator.cginc"
		
		sampler2D _MainTex;
		float3 _Tint;

		float3 _EdgeColor;
		float _EdgeThickness;

		float _CellSize;
		float _TimeScale;

		float _Step;

		struct Input 
		{
			float3 worldPos;
		};

		float3 voronoiNoise(float3 value){
			float3 baseCell = floor(value);

			//first pass to find the closest cell
			float minDistToCell = 10;
			float3 toClosestCell;
			float3 closestCell;
			[unroll]
			for(int x1=-1; x1<=1; x1++){
				[unroll]
				for(int y1=-1; y1<=1; y1++){
					[unroll]
					for(int z1=-1; z1<=1; z1++){
						float3 cell = baseCell + float3(x1, y1, z1);
						float3 cellPosition = cell + rand3dTo3d(cell);
						float3 toCell = cellPosition - value;
						float distToCell = length(toCell);
						if(distToCell < minDistToCell){
							minDistToCell = distToCell;
							closestCell = cell;
							toClosestCell = toCell;
						}
					}
				}
			}

			//second pass to find the distance to the closest edge
			float minEdgeDistance = 10;
			[unroll]
			for(int x2=-1; x2<=1; x2++){
				[unroll]
				for(int y2=-1; y2<=1; y2++){
					[unroll]
					for(int z2=-1; z2<=1; z2++){
						float3 cell = baseCell + float3(x2, y2, z2);
						float3 cellPosition = cell + rand3dTo3d(cell);
						float3 toCell = cellPosition - value;

						float3 diffToClosestCell = abs(closestCell - cell);
						bool isClosestCell = diffToClosestCell.x + diffToClosestCell.y + diffToClosestCell.z < 0.1;
						if(!isClosestCell){
							float3 toCenter = (toClosestCell + toCell) * 0.5;
							float3 cellDifference = normalize(toCell - toClosestCell);
							float edgeDistance = dot(toCenter, cellDifference);
							minEdgeDistance = min(minEdgeDistance, edgeDistance);
						}
					}
				}
			}

			float random = rand3dTo1d(closestCell);
    		return float3(minDistToCell, random, minEdgeDistance);
		}

		void surf (Input i, inout SurfaceOutputStandard o) {
			float3 value = i.worldPos.xyz / _CellSize;
			value.y += _Time.y * _TimeScale;
			float3 noise = voronoiNoise(value);
 
			float valueChange = fwidth(value.y) *  0.5;
			float fragments = 1 - step(valueChange, noise.y - _Step);
			float edge = 1 - step(valueChange, noise.z - _EdgeThickness);

			o.Albedo = fragments * _Tint;
			o.Emission = edge * _EdgeColor;

			o.Alpha = fragments;
			
		}
		ENDCG

		//--------------------------//
		//  Render the back faces   //
		//--------------------------//
		       
		Cull Front
		CGPROGRAM
		#pragma target 3.0
		void vert (inout appdata_full v)
		{
			// Here we are making the surface look
			// the opposite direction
			v.normal = -v.normal;
		}
		ENDCG

		//--------------------------//
		//  Render the front faces  //
		//--------------------------//
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		ENDCG

	}
}
