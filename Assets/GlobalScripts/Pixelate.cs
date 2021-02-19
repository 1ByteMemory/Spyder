using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(PixelateRenderer), PostProcessEvent.AfterStack, "Custom/Pixelate")]
public sealed class Pixelate : PostProcessEffectSettings
{
	public Vector2Parameter ratio = new Vector2Parameter { value = new Vector2(320, 240) };
}

public sealed class PixelateRenderer : PostProcessEffectRenderer<Pixelate>
{
	public override void Render(PostProcessRenderContext context)
	{
		var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixelate"));
		sheet.properties.SetVector("_Ratio", settings.ratio);
		context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
	}
}
