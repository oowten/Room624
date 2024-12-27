using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public Renderer spotlightRenderer1; // 第一個 spotlight 的 Renderer
    public Renderer spotlightRenderer2; // 第二個 spotlight 的 Renderer
    public string shaderOpacityProperty = "_opacity"; // Shader 的透明屬性名稱
    public float spotlightFadeDuration = 3f; // Spotlight 淡入/淡出時間

    [Header("Video Settings")]
    public GameObject videoScreen; // 用於播放影片的物件
    public VideoPlayer videoPlayer; // VideoPlayer 組件
    public float videoFadeDuration = 2f; // 影片淡入/淡出時間

    [Header("Trash Bin Settings")]
    public GameObject trashBin; // 垃圾桶物件
    public GameObject trashBinContents; // 垃圾桶內部內容
    public Animator trashBinAnimator; // 垃圾桶動畫

    [Header("Delays Between Steps")]
    public float delayAfterSpotlight = 5f; // 第一個 spotlight 出現後的延遲
    public float delayAfterVideoStart = 2f; // 影片開始播放後的延遲
    public float delayAfterVideoEnd = 2f; // 影片結束後的延遲
    public float delayForSecondSpotlight = 1f; // 第二個 spotlight 出現的延遲
    public float delayForTrash = 5f;

    private Material videoMaterial; // 影片材質
    private bool hasPlayed = false; // 是否已經執行過場景序列

    void Start()
    {
        // 初始化
        videoMaterial = videoScreen.GetComponent<Renderer>().material;
        SetVideoAlpha(0); // 設置影片初始透明度為 0
        videoScreen.SetActive(false); // 隱藏影片物件
        videoPlayer.prepareCompleted += OnVideoPrepared; // 註冊影片準備完成的事件

        // 確保 spotlight 初始透明度為 0
        SetSpotlightOpacity(spotlightRenderer1, 0);
        SetSpotlightOpacity(spotlightRenderer2, 0);

        // 隱藏垃圾桶及其內容
        trashBin.SetActive(false);
        trashBinContents.SetActive(false);

        // 進入場景後自動執行場景序列
        StartCoroutine(SceneSequence());
    }

    private IEnumerator SceneSequence()
    {
        if (hasPlayed) yield break; // 防止重复调用
        hasPlayed = true;
        // 1. 第一個 spotlight 淡入
        yield return StartCoroutine(FadeSpotlight(spotlightRenderer1, 0, 0.03f, spotlightFadeDuration));
        yield return new WaitForSeconds(delayAfterSpotlight);

        // 2. 顯示影片畫面並準備播放
        videoScreen.SetActive(true);
        videoPlayer.Prepare(); // 開始準備影片
        yield return new WaitUntil(() => videoPlayer.isPrepared); // 等待影片準備完成

        // 淡入影片
        yield return StartCoroutine(FadeVideoAlpha(0, 1, videoFadeDuration));
        videoPlayer.Play(); // 開始播放影片
        yield return new WaitForSeconds(delayAfterVideoStart);

        yield return new WaitForSeconds(delayForTrash);
        // 顯示垃圾桶及其內容
        trashBin.SetActive(true); // 顯示垃圾桶
        trashBinContents.SetActive(true); // 顯示垃圾桶內部內容

        // 等待影片播放結束
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delayAfterVideoEnd);

        // 第一個 spotlight 淡出並銷毀
        yield return StartCoroutine(FadeSpotlight(spotlightRenderer1, 0.07f, 0, spotlightFadeDuration));
        Destroy(spotlightRenderer1.gameObject); // 銷毀第一個 spotlight 的物件

        // 淡出影片
        yield return StartCoroutine(FadeVideoAlpha(1, 0, videoFadeDuration));
        videoScreen.SetActive(false); // 隱藏影片物件

        // 第二個 spotlight 淡入
        yield return new WaitForSeconds(delayForSecondSpotlight);
        yield return StartCoroutine(FadeSpotlight(spotlightRenderer2, 0, 0.07f, spotlightFadeDuration));


        trashBinAnimator.SetBool("isOpen", true);
    }

    private IEnumerator FadeSpotlight(Renderer spotlight, float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float opacity = Mathf.Lerp(start, end, elapsed / duration);
            SetSpotlightOpacity(spotlight, opacity);
            yield return null;
        }
        SetSpotlightOpacity(spotlight, end);
    }

    private void SetSpotlightOpacity(Renderer spotlight, float opacity)
    {
        if (spotlight.material.HasProperty(shaderOpacityProperty))
        {
            spotlight.material.SetFloat(shaderOpacityProperty, opacity);
        }
    }

    private IEnumerator FadeVideoAlpha(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / duration);
            SetVideoAlpha(alpha);
            yield return null;
        }
        SetVideoAlpha(end);
    }

    private void SetVideoAlpha(float alpha)
    {
        if (videoMaterial != null)
        {
            // 確保透明度在合法範圍內
            alpha = Mathf.Clamp01(alpha);

            // 更新材質的顏色透明度
            Color color = videoMaterial.GetColor("_Color");
            color.a = alpha;
            videoMaterial.SetColor("_Color", color);
        }
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        Debug.Log("Video prepared and ready to play.");
    }
}

