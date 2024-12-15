using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public Image fadeImage; // ���̶¦�Ϲ��ޥ�
    public float fadeDuration = 1f; // �H�J�H�X�ɶ�
    public string targetSceneName; // �ؼг����W�١]�i�b Inspector ��g�^

    private void Start()
    {
        // �T�O�����[���ɥߧY����H�J�ĪG
        if (fadeImage != null)
        {
            StartCoroutine(Fade(0f));
        }
        else
        {
            Debug.LogError("fadeImage ���]�m�A�Цb Inspector ���j�w�@�ӥ��̪� UI Image�C");
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
            Debug.LogError("�ؼг����W�٥��]�m�A�Цb Inspector ����g�����W�١I");
        }
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // �H�X
        if (fadeImage != null)
        {
            yield return StartCoroutine(Fade(1f));
        }

        // �[������
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null; // ���ݳ����[������
        }

        // �H�J
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
