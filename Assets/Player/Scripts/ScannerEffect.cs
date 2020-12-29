using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScannerEffect : MonoBehaviour
{
	[Header("Scanner")]
	public Transform ScannerOrigin;
	public Material EffectMaterial;

	public float ScanDistance;
	public float scanSpeed = 100;

	private Camera _camera;

	[HideInInspector]
	public bool _scanning;


	[Header("Edge Effect")]
	//public Material EdgeEffect;
	public float depthScale;
	public float depthBias;
	public float normalScale;
	public float normalBias;

	public float lineThickness;
	public Color lineColor;


	void Update()
	{
		if (_scanning)
		{
			// Increase scanner radius
			ScanDistance += Time.deltaTime * scanSpeed;

			// prevent the numbers from getting too high
			if (ScanDistance >= 500)
			{
				ScanDistance = 0;
				_scanning = false;
			}
		}
	}
	// End Demo Code

	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	public void Scan()
	{
		_scanning = true;
		ScanDistance = 0;
	}
	public void Scan(Vector3 position)
	{
		_scanning = true;
		ScanDistance = 0;
		ScannerOrigin.position = position;
	}


	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
		EffectMaterial.SetFloat("_ScanDistance", ScanDistance);

		SetEdgeProperties();

		RaycastCornerBlit(src, dst, EffectMaterial);
	}

	void SetEdgeProperties()
	{
		EffectMaterial.SetFloat("_DepthScale", depthScale * 100);
		EffectMaterial.SetFloat("_DepthBias", depthBias);
		EffectMaterial.SetFloat("_NormalScale", normalScale);
		EffectMaterial.SetFloat("_NormalBias", normalBias);
		
		EffectMaterial.SetFloat("_Thickness", lineThickness / 1000);
		EffectMaterial.SetVector("_Color", lineColor);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _camera.farClipPlane;
		float camFov = _camera.fieldOfView;
		float camAspect = _camera.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_camera.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_MainTex", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}

}
