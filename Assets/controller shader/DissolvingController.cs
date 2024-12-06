using System.Collections;
using UnityEngine;

public class DissolvingController : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public ParticleSystem particleSystem; // �ɤl�t��
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    private Material[] skinnedMaterials;

    private bool isDissolving = false;

    private ParticleSystem.EmissionModule emissionModule; // �ɤl�o�g�]�w

    [Header("�i�վ㪺�ɤl�ƶq")]
    [Range(1, 1000)] // �]�w�d��A�o�˦b Inspector ���i�վ�
    public float particleCount = 100f; // �ɤl�ƶq

    void Start()
    {
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;

            // �إߧ��誺�W�߹�ҡA�T�O�C�Ӫ��󳣦��W�ߪ� dissolve ����
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // ����ɤl�ĪG�A�קK�i�J Play Mode �ɦ۰ʼ���
        if (particleSystem != null)
        {
            particleSystem.Stop();
            emissionModule = particleSystem.emission; // ���o�o�g�Ҷ�
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log ��X�T���A�ˬd�i�JĲ�o�ϰ쪺����
        Debug.Log("����i�JĲ�o�ϰ�: " + other.gameObject.name);

        // �ˬd�O�_�i�J�c�l��Ĳ�o�ϰ�A�åB����O�ݭn dissolve ��
        if (!isDissolving && other.CompareTag("Dissolvable"))
        {
            Debug.Log("�}�l���� Dissolve �M�ɤl�ĪG");
            StartCoroutine(DissolveCo());
        }
    }

    IEnumerator DissolveCo()
    {
        isDissolving = true;

        // �b�}�l dissolve �e����ɤl�ĪG
        if (particleSystem != null)
        {
            Debug.Log("����ɤl�ĪG");
            particleSystem.Play(); // ����ɤl�t��

            // �]�w�ɤl�o�g�t�v�A�ھ� particleCount �վ�
            emissionModule.rateOverTime = particleCount; // �ھڳ]�w���ɤl�ƶq�վ�o�g�t�v
        }

        if (skinnedMaterials.Length > 0)
        {
            float counter = 0;

            while (counter < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_DissolveAmount", counter); // ���� dissolve ���i��
                }
                yield return new WaitForSeconds(refreshRate); // �]�w��s�t��
            }
        }

        // �b dissolve �����ᰱ��ɤl�ĪG
        if (particleSystem != null)
        {
            // ����ɤl�t��
            emissionModule.rateOverTime = 0f; // ����o�g�ɤl
            Debug.Log("����ɤl�ĪG");
            particleSystem.Stop();
        }

        isDissolving = false;
    }
}
