using Apps;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingMainScene : MonoBehaviour
{
    private void Start()
    {
        AppsIntegration.Initialize();

        StartCoroutine(LoadMainSceneAsn());
    }
 
    private IEnumerator LoadMainSceneAsn()
    {
        while (!AppsIntegration.IsInited)
            yield return null;

        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
