using UnityEngine;
using System.Collections;

public class BoxTrigger : MonoBehaviour
{
    public ParticleSystem dissolveParticles; // �ɤl�ĪG
    public string targetTag = "Dissolvable"; // �˴����������

    void OnTriggerEnter(Collider other)
    {
        // �˴��i�J������O�_�����T������
        if (other.CompareTag(targetTag))
        {
            Debug.Log("����i�J�˴��ϰ�G" + other.name);

            // �}�l���添�ѮĪG
            StartCoroutine(TriggerDissolveEffect(other.gameObject));
        }
    }

    private IEnumerator TriggerDissolveEffect(GameObject obj)
    {
        // �T�O���� Renderer �M����
        Renderer objRenderer = obj.GetComponent<Renderer>();
        if (objRenderer == null)
        {
            Debug.LogWarning("����S�� Renderer�A�L�k���� Dissolve �ĪG");
            yield break;
        }

        Material objMaterial = objRenderer.material;

        // ��l�� Dissolve �ĪG�Ѽ�
        float dissolveAmount = 0;
        while (dissolveAmount < 1)
        {
            dissolveAmount += Time.deltaTime * 0.5f; // ����ѳt��
            objMaterial.SetFloat("_DissolveAmount", dissolveAmount); // Shader ���� Dissolve �Ѽ�
            yield return null;
        }

        // ����ɤl�ĪG
        if (dissolveParticles != null)
        {
            dissolveParticles.transform.position = obj.transform.position;
            dissolveParticles.Play();
        }

        // ���éΧR������
        obj.SetActive(false);
    }
}
