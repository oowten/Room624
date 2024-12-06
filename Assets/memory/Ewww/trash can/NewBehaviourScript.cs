using UnityEngine;
using System.Collections;

public class BoxTrigger : MonoBehaviour
{
    public ParticleSystem dissolveParticles; // 粒子效果
    public string targetTag = "Dissolvable"; // 檢測的物件標籤

    void OnTriggerEnter(Collider other)
    {
        // 檢測進入的物件是否有正確的標籤
        if (other.CompareTag(targetTag))
        {
            Debug.Log("物件進入檢測區域：" + other.name);

            // 開始執行溶解效果
            StartCoroutine(TriggerDissolveEffect(other.gameObject));
        }
    }

    private IEnumerator TriggerDissolveEffect(GameObject obj)
    {
        // 確保物件有 Renderer 和材質
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogWarning("物件沒有 Renderer，無法應用 Dissolve 效果");
            yield break;
        }

        Material objMaterial = objRenderer.material;

        // 初始化 Dissolve 效果參數
        float dissolveAmount = 0;
        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * 0.5f; // 控制溶解速度
            objMaterial.SetFloat("_DissolveAmount", dissolveAmount); // Shader 中的 Dissolve 參數
            yield return null;
        }

        // 播放粒子效果
        if (dissolveParticles != null)
        {
            dissolveParticles.transform.position = obj.transform.position;
            dissolveParticles.Play();
        }

        // 隱藏或刪除物件
        obj.SetActive(false);
    }
}
