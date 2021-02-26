using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    public static string sceneToLoad;
    public TextMeshProUGUI sceneName;

    // Start is called before the first frame update
    void Start()
    {
        if (sceneName != null)
		{
            sceneName.text = sceneToLoad;
		}
		else
		{
            sceneName.text = "";
		}
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
