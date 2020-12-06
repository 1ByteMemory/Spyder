using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DigitalOverlay : MonoBehaviour
{

    public Material digitalOverleyEffect;
	public new bool enabled;
	public float depthScale;


	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (digitalOverleyEffect != null && enabled == true)
		{
			GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;

			digitalOverleyEffect.SetFloat("_Scale", depthScale);
			Graphics.Blit(source, destination, digitalOverleyEffect);
		}

		RenderTexture.active = destination;
	}
}
