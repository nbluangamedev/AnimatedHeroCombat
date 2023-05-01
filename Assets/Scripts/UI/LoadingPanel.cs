using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public TextMeshProUGUI loadingPercentText;
    public Slider loadingSlider;

    void OnEnable()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level1");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress;
            loadingPercentText.SetText($"LOADING SCENES: {asyncOperation.progress * 100}%");
            if (asyncOperation.progress >= 0.9f)
            {
                loadingSlider.value = 1f;
                loadingPercentText.SetText("Touch the screen to continue");
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0) || Input.touchCount!=0)
                {
                    asyncOperation.allowSceneActivation = true;
                    if (UIManager.HasInstance)
                    {
                        UIManager.Instance.ActiveGamePanel(true);
                        UIManager.Instance.ActiveLoadingPanel(false);
                        UIManager.Instance.ActiveMobilePanel(true);
                    }
                    if (GameManager.HasInstance)
                    {
                        GameManager.Instance.StartGame();
                    }
                }
            }
            yield return null;
        }
    }
}