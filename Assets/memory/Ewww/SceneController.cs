using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public Renderer spotlightRenderer; // 聚光燈物件的 Renderer
    public string shaderOpacityProperty = "_opacity"; // Shader 中控制透明度的屬性名稱
    public float spotlightFadeDuration = 3f; // 聚光燈淡入時間

    [Header("Video Settings")]
    public GameObject videoScreen; // 綁定影片材質的物件
    public VideoPlayer videoPlayer; // VideoPlayer 組件
    public float videoFadeDuration = 2f; // 影片淡入淡出時間

    [Header("Trash Bin Settings")]
    public Animator trashBinAnimator; // 垃圾桶動畫

    [Header("Delays Between Steps")]
    public float delayAfterSpotlight = 2f; // 聚光燈完成後的延遲
    public float delayAfterVideoStart = 2f; // 影片播放開始後的延遲
    public float delayAfterVideoEnd = 2f; // 影片播放結束後的延遲

    private Material videoMaterial; // 影片的材質
    private bool hasPlayed = false; // 控制是否已經播放過流程

    void Start()
    {
        // 初始化材質
        videoMaterial = videoScreen.GetComponent<Renderer>().material;
        SetVideoAlpha(0); // 設置初始透明度為 0
        videoScreen.SetActive(false); // 隱藏影片螢幕
        videoPlayer.prepareCompleted += OnVideoPrepared; // 註冊影片準備完成的回調
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(SceneSequence()); // 開始整個流程
        }
    }

    private IEnumerator SceneSequence()
    {
        // 1. 聚光燈透明度逐漸上升
        yield return StartCoroutine(FadeSpotlight(0, 0.17f, spotlightFadeDuration));
        yield return new WaitForSeconds(delayAfterSpotlight);

        // 2. 顯示影片螢幕並準備播放
        videoScreen.SetActive(true);
        videoPlayer.Prepare(); // 開始準備影片
        yield return new WaitUntil(() => videoPlayer.isPrepared); // 等待影片準備完成

        // 淡入影片螢幕
        yield return StartCoroutine(FadeVideoAlpha(0, 1, videoFadeDuration));
        videoPlayer.Play(); // 開始播放影片
        yield return new WaitForSeconds(delayAfterVideoStart);

        // 等待影片播放完成
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delayAfterVideoEnd);

        // 淡出影片螢幕
        yield return StartCoroutine(FadeVideoAlpha(1, 0, videoFadeDuration));
        videoScreen.SetActive(false); // 隱藏影片螢幕

        // 3. 播放垃圾桶動畫
        trashBinAnimator.SetBool("isOpen", true);
    }

    private IEnumerator FadeSpotlight(float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float opacity = Mathf.Lerp(start, end, elapsed / duration);
            SetSpotlightOpacity(opacity);
            yield return null;
        }
        SetSpotlightOpacity(end);
    }

    private void SetSpotlightOpacity(float opacity)
    {
        if (spotlightRenderer.material.HasProperty(shaderOpacityProperty))
        {
            spotlightRenderer.material.SetFloat(shaderOpacityProperty, opacity);
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
            // 確保 alpha 值在範圍內
            alpha = Mathf.Clamp01(alpha);

            // 取得材質的顏色並更新透明度
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
