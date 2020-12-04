Shader "Custom/ColoredOutlineA"
{
	Properties
	{
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_Strength("EdgeStrength", Float) = 1

		_CellSize("Cell Size", Range(0,2)) = 2
		_CellOffset("Cell Offset", Float) = 1
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

		CGPROGRAM

		#pragma target 3.0
		#pragma surface surf Standard fullforwardshadows

		#include "RandomGenerator.cginc"

		struct Input
		{
			float3 worldPos;
		};
			
		float _CellSize;
		float _CellOffset;

		float voronoiNoise(float2 value)
		{
			float2 baseCell = floor(value);

			float minDistToCell = 10;
			float2 closestCell;
			[unroll]
			for	(int x = -1; x <= 1; x++)
			{
				[unroll]
				for (int y = -1; y <= 1; y++)
				{
					float2 cell = baseCell + float2(x, y);
					float2 cellPosition = cell + rand2dTo2d(cell);
					float2 toCell = cellPosition - value;
					float distToCell = length(toCell);
					if (distToCell < minDistToCell)
					{
						minDistToCell = distToCell;
						closestCell = cell;
					}
				}
			}
			float random = rand2dTo1d(closestCell);
			return float2(minDistToCell, random);
		}

		void surf (Input i, inout SurfaceOutputStandard o)
		{
			float2 value = i.worldPos.xz / _CellSize;
			float noise = voronoiNoise(value);

			o.Albedo = noise;
		}
		ENDCG
	}
}
