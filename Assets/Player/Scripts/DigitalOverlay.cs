using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DigitalOverlay : MonoBehaviour
{

    public Material digitalOverleyEffect;
	public new bool enabled;
	public float depthScale;
	public float lineThickness;
	public Color lineColor;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (digitalOverleyEffect != null && enabled == true)
		{
			GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;

			digitalOverleyEffect.SetFloat("_Scale", depthScale);
			digitalOverleyEffect.SetFloat("_Thickness", lineThickness);
			digitalOverleyEffect.SetVector("_Color", lineColor);
			Graphics.Blit(source, destination, digitalOverleyEffect);
		}

		RenderTexture.active = destination;
	}
}
