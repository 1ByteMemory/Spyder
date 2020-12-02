using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class XRayReplacement : MonoBehaviour
{
    public Shader XRayShader;

    private Material _compositeMat;


    void OnEnable()
    {
        //_compositeMat = new Material(Shader.Find("Custom/InactiveDimension"));
        GetComponent<Camera>().SetReplacementShader(XRayShader, "XRay");
    }



	private void OnDisable()
	{
        //_compositeMat = null;
        GetComponent<Camera>().ResetReplacementShader();
	}
}