using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DigitalOverlay : MonoBehaviour
{

    public Material digitalOverleyEffect;
	public new bool enabled;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (digitalOverleyEffect != null && enabled == true)
			Graphics.Blit(source, destination, digitalOverleyEffect);
	}
}
