using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReplacmentShader : MonoBehaviour
{

	public Shader inactiveShader;


	void OnEnable()
	{
		if (inactiveShader != null)
			GetComponent<Camera>().SetReplacementShader(inactiveShader, "Switchable");
	}

	private void OnDisable()
	{
		GetComponent<Camera>().ResetReplacementShader();
	}
}
