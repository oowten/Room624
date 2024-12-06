using System.Collections;
using UnityEngine;

public class DissolvingController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public ParticleSystem particleSystem; // 粒子系統
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material[] skinnedMaterials;

    private bool isDissolving = false;

    private ParticleSystem.EmissionModule emissionModule; // 粒子發射設定

    [Header("可調整的粒子數量")]
    [Range(1, 1000)] // 設定範圍，這樣在 Inspector 中可調整
    public float particleCount = 100f; // 粒子數量

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;

            // 建立材質的獨立實例，確保每個物件都有獨立的 dissolve 控制
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // 停止粒子效果，避免進入 Play Mode 時自動播放
        if (particleSystem != null)
        {
            particleSystem.Stop();
            emissionModule = particleSystem.emission; // 取得發射模塊
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log 輸出訊息，檢查進入觸發區域的物體
        Debug.Log("物體進入觸發區域: " + other.gameObject.name);

        // 檢查是否進入箱子的觸發區域，並且物體是需要 dissolve 的
        if (!isDissolving && other.CompareTag("Dissolvable"))
        {
            Debug.Log("開始執行 Dissolve 和粒子效果");
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        isDissolving = true;

        // 在開始 dissolve 前播放粒子效果
        if (particleSystem != null)
        {
            Debug.Log("播放粒子效果");
            particleSystem.Play(); // 播放粒子系統

            // 設定粒子發射速率，根據 particleCount 調整
            emissionModule.rateOverTime = particleCount; // 根據設定的粒子數量調整發射速率
        }

        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;

            while (counter < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter); // 控制 dissolve 的進度
                }
                yield return new WaitForSeconds(refreshRate); // 設定刷新速度
            }
        }

        // 在 dissolve 完成後停止粒子效果
        if (particleSystem != null)
        {
            // 停止粒子系統
            emissionModule.rateOverTime = 0f; // 停止發射粒子
            Debug.Log("停止粒子效果");
            particleSystem.Stop();
        }

        isDissolving = false;
    }
}
