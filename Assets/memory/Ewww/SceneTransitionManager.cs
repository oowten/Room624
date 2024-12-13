using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // 全屏黑色圖像引用
    public float fadeDuration = 1f; // 淡入淡出時間

    private void Start()
    {
        // 確保場景加載時立即執行淡入效果
        StartCoroutine(Fade(0f));
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene("vr"));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // 淡出
        yield return StartCoroutine(Fade(1f));

        // 加載場景
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("vr");
        while (!asyncLoad.isDone)
        {
            yield return null; // 等待場景加載完成
        }

        // 淡入
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
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
