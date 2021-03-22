using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class ScannerEffect : MonoBehaviour
{
	[Header("Scanner")]
	public Transform ScannerOrigin;
	public Material scanEffect;
	public Material fadeEffect;

	Material EffectMaterial;

	public float MaxScanDist = 200;
	public float ScanDistance;
	public float scanSpeed = 100;
	public float fadeSpeed = 1;

	private Camera _camera;

	[HideInInspector]
	public bool _scanning;

	private Dimension currDimension;

	[Header("Edge Effect")]
	//public Material EdgeEffect;
	public float depthScale;
	public float depthBias;
	public float normalScale;
	public float normalBias;

	public float lineThickness;
	public Color lineColor;
	public Color secondaryColor;


	[Header("Furthest Point")]
	private Vector2 scrPos;
	private bool hasFurthestPoint;

	[HideInInspector]
	public bool epilepsySafeMode;

	void Update()
	{
		if (epilepsySafeMode)
		{
			EffectMaterial = fadeEffect;
			if (_scanning)
			{
				switch (currDimension)
				{
					case Dimension.Digital:
						ScanDistance = ScanDistance <= 0 ? 0 : ScanDistance;
						ScanDistance += Time.deltaTime * fadeSpeed;
						break;
					case Dimension.Real:
						ScanDistance = ScanDistance >= 1 ? 1 : ScanDistance;
						ScanDistance -= Time.deltaTime * fadeSpeed;
						break;
				}

				if (ScanDistance >= 1 || ScanDistance <= 0)
				{
					ScanDistance = Mathf.Clamp01(ScanDistance);
					_scanning = false;
				}
			}
		}
		else
		{
			EffectMaterial = scanEffect;
			if (_scanning)
			{
				// Increase or decrease scanner radius
				switch (currDimension)
				{
					case Dimension.Digital:
						ScanDistance = ScanDistance < 0 ? 0 : ScanDistance;
						ScanDistance += Time.deltaTime * scanSpeed;
						hasFurthestPoint = false;
						break;
					case Dimension.Real:
						if (!hasFurthestPoint)
						{
							ScanDistance = GetFurthestPoint();
							hasFurthestPoint = true;
						}

						ScanDistance -= Time.deltaTime * scanSpeed;
						break;
				}

				// prevent the numbers from getting too high
				if (ScanDistance >= MaxScanDist || ScanDistance <= 0)
				{
					//ScanDistance = 0;
					_scanning = false;
				}
			}
		}
	}
	// End Demo Code

	void OnEnable()
	{
		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	private void OnDestroy()
	{
		EffectMaterial.SetFloat("_ScanDistance", 0);
	}


	private float GetFurthestPoint()
	{
		float maxDist = 0;
		for (float y = 0; y < 1; y += 0.01f)
		{
			for (float x = 0; x < 1; x += 0.01f)
			{
				scrPos = new Vector2(x * _camera.pixelWidth, y * _camera.pixelHeight);
				Ray ray = _camera.ScreenPointToRay(scrPos);
				Physics.Raycast(ray, out RaycastHit hitInfo, 500);
				if (hitInfo.distance > maxDist) maxDist = hitInfo.distance;
			}
		}
		return maxDist;
	}

	public void Scan(Dimension dimension)
	{
		currDimension = dimension;
		_scanning = true;
		//ScanDistance = 0;
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
		SetEdgeProperties();
		if (!epilepsySafeMode)
		{
			EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
			EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
			RaycastCornerBlit(src, dst, EffectMaterial);
		}
		else
		{
			EffectMaterial.SetFloat("_Fade", ScanDistance);

			Graphics.Blit(src, dst, EffectMaterial);
		}
	}

	void SetEdgeProperties()
	{
		EffectMaterial.SetFloat("_DepthScale", depthScale * 100);
		EffectMaterial.SetFloat("_DepthBias", depthBias);
		EffectMaterial.SetFloat("_NormalScale", normalScale);
		EffectMaterial.SetFloat("_NormalBias", normalBias);
		
		EffectMaterial.SetFloat("_Thickness", lineThickness / 1000);
		EffectMaterial.SetVector("_Color", lineColor);
		EffectMaterial.SetVector("_SecondaryColor", secondaryColor);
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
