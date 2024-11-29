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

            // �إߧ��誺�W�߹�ҡA�T�O�C�Ӫ��󳣦��W�ߪ�dissolve����
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // ����ɤl�ĪG�A�קK�i�JPlay Mode�ɦ۰ʼ���
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

        // �b�}�ldissolve�e����ɤl�ĪG
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

        // �bdissolve�����ᰱ��ɤl�ĪG
        if (VFXGraph != null)
        {
            VFXGraph.Stop();
        }

        isDissolving = false;
    }
}
