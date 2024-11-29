using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolvingController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public VisualEffect VFXGraph;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material[] skinnedMaterials;

    private bool isDissolving = false;

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;

            // 建立材質的獨立實例，確保每個物件都有獨立的dissolve控制
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // 停止粒子效果，避免進入Play Mode時自動播放
        if (VFXGraph != null)
        {
            VFXGraph.Stop();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDissolving)
        {
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        isDissolving = true;

        // 在開始dissolve前播放粒子效果
        if (VFXGraph != null)
        {
            VFXGraph.Play();
        }

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
        }

        // 在dissolve完成後停止粒子效果
        if (VFXGraph != null)
        {
            VFXGraph.Stop();
        }

        isDissolving = false;
    }
}
