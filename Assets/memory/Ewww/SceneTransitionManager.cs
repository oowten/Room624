using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // 全屏黑色圖像引用
    public float fadeDuration = 1f; // 淡入淡出時間
    public string targetSceneName; // 目標場景名稱（可在 Inspector 填寫）

    private void Start()
    {
        // 確保場景加載時立即執行淡入效果
        if (fadeImage != null)
        {
            StartCoroutine(Fade(0f));
        }
        else
        {
            Debug.LogError("fadeImage 未設置，請在 Inspector 中綁定一個全屏的 UI Image。");
        }
    }

    public void TransitionToScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            StartCoroutine(FadeOutAndLoadScene(targetSceneName));
        }
        else
        {
            Debug.LogError("目標場景名稱未設置，請在 Inspector 中填寫場景名稱！");
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // 淡出
        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(1f));
        }

        // 加載場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; // 等待場景加載完成
        }

        // 淡入
        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(0f));
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
        {
            yield break;
        }

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
