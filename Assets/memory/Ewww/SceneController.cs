using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class SceneController : MonoBehaviour
{
    [Header("Spotlight Settings")]
    public Renderer spotlightRenderer; // �E���O���� Renderer
    public string shaderOpacityProperty = "_opacity"; // Shader ������z���ת��ݩʦW��
    public float spotlightFadeDuration = 3f; // �E���O�H�J�ɶ�

    [Header("Video Settings")]
    public GameObject videoScreen; // �j�w�v�����誺����
    public VideoPlayer videoPlayer; // VideoPlayer �ե�
    public float videoFadeDuration = 2f; // �v���H�J�H�X�ɶ�

    [Header("Trash Bin Settings")]
    public Animator trashBinAnimator; // �U�����ʵe

    [Header("Delays Between Steps")]
    public float delayAfterSpotlight = 2f; // �E���O�����᪺����
    public float delayAfterVideoStart = 2f; // �v������}�l�᪺����
    public float delayAfterVideoEnd = 2f; // �v�����񵲧��᪺����

    private Material videoMaterial; // �v��������
    private bool hasPlayed = false; // ����O�_�w�g����L�y�{

    void Start()
    {
        // ��l�Ƨ���
        videoMaterial = videoScreen.GetComponent<Renderer>().material;
        SetVideoAlpha(0); // �]�m��l�z���׬� 0
        videoScreen.SetActive(false); // ���üv���ù�
        videoPlayer.prepareCompleted += OnVideoPrepared; // ���U�v���ǳƧ������^��
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasPlayed)
        {
            hasPlayed = true;
            StartCoroutine(SceneSequence()); // �}�l��Ӭy�{
        }
    }

    private IEnumerator SceneSequence()
    {
        // 1. �E���O�z���׳v���W��
        yield return StartCoroutine(FadeSpotlight(0, 0.5f, spotlightFadeDuration));
        yield return new WaitForSeconds(delayAfterSpotlight);

        // 2. ��ܼv���ù��÷ǳƼ���
        videoScreen.SetActive(true);
        videoPlayer.Prepare(); // �}�l�ǳƼv��
        yield return new WaitUntil(() => videoPlayer.isPrepared); // ���ݼv���ǳƧ���

        // �H�J�v���ù�
        yield return StartCoroutine(FadeVideoAlpha(0, 1, videoFadeDuration));
        videoPlayer.Play(); // �}�l����v��
        yield return new WaitForSeconds(delayAfterVideoStart);

        // ���ݼv�����񧹦�
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delayAfterVideoEnd);

        // �H�X�v���ù�
        yield return StartCoroutine(FadeVideoAlpha(1, 0, videoFadeDuration));
        videoScreen.SetActive(false); // ���üv���ù�

        // 3. ����U�����ʵe
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
            // �T�O alpha �Ȧb�d��
            alpha = Mathf.Clamp01(alpha);

            // ���o���誺�C��ç�s�z����
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
