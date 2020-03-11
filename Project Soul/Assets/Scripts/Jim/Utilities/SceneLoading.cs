using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    private Image _progressBar;

    void Start()
    {
        StartCoroutine(LoadAsyncOperation());
    }

    IEnumerator LoadAsyncOperation()
    {
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(2);
        
        while(gameLevel.progress < 1)
        {
            _progressBar.fillAmount = gameLevel.progress;
            yield return new WaitForEndOfFrame();
        }

    }
}
