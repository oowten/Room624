using UnityEngine;
using UnityEngine.SceneManagement; // 用於場景轉場
using System.Collections;

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
    [Range(1, 1000)]
    public float particleCount = 100f; // 粒子數量

    [Header("動畫相關設定")]
    public Animator targetAnimator; // 綁定的 Animator
    public string triggerName = "FlyyyTrigger"; // 觸發動畫的 Trigger 名稱
    public float dissolveDelay = 2f; // 動畫播放後延遲的時間

    [Header("轉場管理器")]
    public SceneTransitionManager sceneTransitionManager; // 轉場管理器引用

    void Start()
    {
        // 初始化材質
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;

            // 建立材質的獨立實例
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // 停止粒子效果
        if (particleSystem != null)
        {
            particleSystem.Stop();
            emissionModule = particleSystem.emission;
        }

        // 檢查是否有綁定 Animator
        if (targetAnimator == null)
        {
            Debug.LogError("未綁定 Animator！請在 Inspector 中將目標 GameObject 的 Animator 拖入。");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("物體進入觸發區域: " + other.gameObject.name);

        // 如果進入的物件具有標籤 "Able"
        if (!isDissolving && other.CompareTag("Able"))
        {
            Debug.Log("開始播放動畫和執行延遲效果");

            // 播放動畫
            if (targetAnimator != null)
            {
                Debug.Log("播放 Flyyy 動畫");
                if (targetAnimator.HasParameterOfType(triggerName, AnimatorControllerParameterType.Trigger))
                {
                    targetAnimator.SetTrigger(triggerName);
                }
                else
                {
                    Debug.LogError($"Animator 中缺少名為 {triggerName} 的 Trigger 參數！");
                }
            }
            else
            {
                Debug.LogError("物件缺少 Animator 組件，無法播放動畫！");
            }

            // 開始延遲後的 Dissolve 效果
            StartCoroutine(DelayedDissolve());

            // 延遲後讓進入的 Able 物件消失
            StartCoroutine(DelayedDestroy(other.gameObject, dissolveDelay));
        }
    }

    IEnumerator DelayedDissolve()
    {
        isDissolving = true;

        // 等待指定的延遲時間
        yield return new WaitForSeconds(dissolveDelay);

        // 播放粒子效果
        if (particleSystem != null)
        {
            Debug.Log("播放粒子效果");
            particleSystem.Play();
            emissionModule.rateOverTime = particleCount;
        }

        // 開始 Dissolve 效果
        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;

            while (counter < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }

            // 確保物件完全透明
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i].SetFloat("_DissolveAmount", 1.0f);
            }
        }


        // 隱藏物件形體（禁用 Renderer 或者刪除）
        if (skinnedMesh != null)
        {
            skinnedMesh.enabled = false; // 禁用 Mesh Renderer，讓物件不可見
        }

        // 停止粒子效果
        if (particleSystem != null)
        {
            emissionModule.rateOverTime = 0f;
            Debug.Log("停止粒子效果");
            particleSystem.Stop();
        }



        // 延遲幾秒後刪除自身
        float destroyDelay = 2f; // 設定刪除自身的延遲時間（確保粒子效果播放完全）
        yield return new WaitForSeconds(destroyDelay);

        Debug.Log("刪除執行動畫的物件");
        Destroy(gameObject); // 刪除當前執行動畫和 Dissolve 的物件本身

        // 轉場
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.TransitionToScene(); // 呼叫場景轉場方法
        }
        else
        {
            Debug.LogError("轉場管理器 (SceneTransitionManager) 未設置！");
        }

        isDissolving = false;
    }

    IEnumerator DelayedDestroy(GameObject targetObject, float delay)
    {
        // 等待指定的時間
        yield return new WaitForSeconds(delay);

        // 刪除進入的物件
        if (targetObject != null)
        {
            Debug.Log($"刪除物件: {targetObject.name}");
            Destroy(targetObject);
        }
    }
}

// 擴展方法檢查 Animator 參數
public static class AnimatorExtensions
{
    public static bool HasParameterOfType(this Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == type)
                return true;
        }
        return false;
    }
}
