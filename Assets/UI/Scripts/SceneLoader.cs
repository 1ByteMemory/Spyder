using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	public string loadingScreen = "LoadingScreen";

    public void LoadScene(string scene)
	{
		LoadingScreen.sceneToLoad = scene;
		SceneManager.LoadScene(loadingScreen);
	}

	public static void Load_Scene(string levelName)
	{
		LoadingScreen.sceneToLoad = levelName;
		SceneManager.LoadScene("LoadingScreen");
	}

	public void ReloadCurrentScene()
	{
		LoadingScreen.sceneToLoad = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(loadingScreen);
	}
}
