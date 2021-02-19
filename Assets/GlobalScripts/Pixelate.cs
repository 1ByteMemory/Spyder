using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Custom/Pixelate")]
public sealed class Pixelate : PostProcessEffectSettings
{
	//[Range(0f, 10f)]
	public FloatParameter blend = new FloatParameter { value = 0.5f };
	public Vector3Parameter color = new Vector3Parameter { value = new Vector3(1, 1, 1) };
}

public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
{
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixelate"));
		sheet.properties.SetFloat("_Blend", settings.blend);
		sheet.properties.SetVector("_Color", settings.color);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
