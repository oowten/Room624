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
    [Range(1, 1000)]
    public float particleCount = 100f; // �ɤl�ƶq

    [Header("�ʵe�����]�w")]
    public Animator targetAnimator; // �j�w�� Animator
    public string triggerName = "FlyyyTrigger"; // Ĳ�o�ʵe�� Trigger �W��
    public float dissolveDelay = 2f; // �ʵe����᩵�𪺮ɶ�

    void Start()
    {
        // ��l�Ƨ���
        if (skinnedMesh != null)
        {
            skinnedMaterials = skinnedMesh.materials;

            // �إߧ��誺�W�߹��
            for (int i = 0; i < skinnedMaterials.Length; i++)
            {
                skinnedMaterials[i] = new Material(skinnedMaterials[i]);
            }
            skinnedMesh.materials = skinnedMaterials;
        }

        // ����ɤl�ĪG
        if (particleSystem != null)
        {
            particleSystem.Stop();
            emissionModule = particleSystem.emission;
        }

        // �ˬd�O�_���j�w Animator
        if (targetAnimator == null)
        {
            Debug.LogError("���j�w Animator�I�Цb Inspector ���N�ؼ� GameObject �� Animator ��J�C");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("����i�JĲ�o�ϰ�: " + other.gameObject.name);

        // �p�G�i�J������㦳���� "Able"
        if (!isDissolving && other.CompareTag("Able"))
        {
            Debug.Log("�}�l����ʵe�M���橵��ĪG");

            // ����ʵe
            if (targetAnimator != null)
            {
                Debug.Log("���� Flyyy �ʵe");
                if (targetAnimator.HasParameterOfType(triggerName, AnimatorControllerParameterType.Trigger))
                {
                    targetAnimator.SetTrigger(triggerName);
                }
                else
                {
                    Debug.LogError($"Animator ���ʤ֦W�� {triggerName} �� Trigger �ѼơI");
                }
            }
            else
            {
                Debug.LogError("����ʤ� Animator �ե�A�L�k����ʵe�I");
            }

            // �}�l����᪺ Dissolve �ĪG
            StartCoroutine(DelayedDissolve());
        }
    }

    IEnumerator DelayedDissolve()
    {
        isDissolving = true;

        // ���ݫ��w������ɶ�
        yield return new WaitForSeconds(dissolveDelay);

        // ����ɤl�ĪG
        if (particleSystem != null)
        {
            Debug.Log("����ɤl�ĪG");
            particleSystem.Play();
            emissionModule.rateOverTime = particleCount;
        }

        // �}�l Dissolve �ĪG
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

        // ����ɤl�ĪG
        if (particleSystem != null)
        {
            emissionModule.rateOverTime = 0f;
            Debug.Log("����ɤl�ĪG");
            particleSystem.Stop();
        }

        isDissolving = false;
    }
}

// �X�i��k�ˬd Animator �Ѽ�
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
