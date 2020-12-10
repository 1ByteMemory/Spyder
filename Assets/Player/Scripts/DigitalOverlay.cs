using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DigitalOverlay : MonoBehaviour
{

    public Material digitalOverleyEffect;
	public new bool enabled;
	public float depthScale;
	public float depthBias;
	public float normalScale;
	public float normalBias;

	public float lineThickness;
	public Color lineColor;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (digitalOverleyEffect != null && enabled == true)
		{
			GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;

			digitalOverleyEffect.SetFloat("_DepthScale", depthScale * 100);
			digitalOverleyEffect.SetFloat("_DepthBias", depthBias);
			digitalOverleyEffect.SetFloat("_NormalScale", normalScale);
			digitalOverleyEffect.SetFloat("_NormalBias", normalBias);

			digitalOverleyEffect.SetFloat("_Thickness", lineThickness / 1000);
			digitalOverleyEffect.SetVector("_Color", lineColor);
			Graphics.Blit(source, destination, digitalOverleyEffect);
		}

		RenderTexture.active = destination;
	}
}
