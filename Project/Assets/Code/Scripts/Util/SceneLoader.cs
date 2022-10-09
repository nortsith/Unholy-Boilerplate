using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float delay = 5f;
    IEnumerator Load(string sceneName)
    {
        PlayerPrefs.SetString("level", sceneName);

        yield return new WaitForSeconds(delay);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Load(sceneName));
    }
}
