using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingPanel;
    public GameObject loadingProgressBar;


    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            //Debug.Log(progress);
            loadingProgressBar.GetComponent<Image>().fillAmount = progress;
            yield return null;
        }
    }

}
