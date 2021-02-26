using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public string loadingScreen;

    public void LoadScene(string scene)
	{
		LoadingScreen.sceneToLoad = scene;
		SceneManager.LoadScene(loadingScreen);
	}

}
