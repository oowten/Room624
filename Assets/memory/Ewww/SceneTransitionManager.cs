using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // ���̶¦�Ϲ��ޥ�
    public float fadeDuration = 1f; // �H�J�H�X�ɶ�

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoadScene("vr"));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // �H�X
        yield return StartCoroutine(Fade(1f));
        // �[������
        SceneManager.LoadScene("vr");
        // �H�J
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
