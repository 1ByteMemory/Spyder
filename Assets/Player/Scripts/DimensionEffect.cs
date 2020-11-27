using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionEffect : MonoBehaviour
{
	Transform ScannerOrigin;
	Material EffectMaterial;
	ScannerEffect scannerEffect;

	public Material inactiveDimensionMaterial;
	public Material activeDimensionMaterial;
	Material activeMat;

	public Dimension camDimension;

	float ScanDistance;
	float scanSpeed = 100;

	private Camera _camera;

	// Demo Code
	bool _scanning;


	void Update()
	{
		_scanning = scannerEffect._scanning;

		if (camDimension == GameManager.currentActiveDimension)
		{
			activeMat = inactiveDimensionMaterial;
		}
		else
		{
			activeMat = activeDimensionMaterial;
		}

		if (_scanning)
		{
			ScanDistance = scannerEffect.ScanDistance;
			scanSpeed = scannerEffect.scanSpeed;

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
		scannerEffect = Camera.main.GetComponent<ScannerEffect>();
		ScannerOrigin = scannerEffect.ScannerOrigin;


		_camera = GetComponent<Camera>();
		_camera.depthTextureMode = DepthTextureMode.Depth;
	}

	
	[ImageEffectOpaque]
	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		//EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);
		//EffectMaterial.SetFloat("_ScanDistance", ScanDistance);
		
		if (activeMat != null)
		{
			RaycastCornerBlit(src, dst, activeMat);
		}
		
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
