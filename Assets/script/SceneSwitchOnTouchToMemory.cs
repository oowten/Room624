using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchOnTouchToMemory : MonoBehaviour
{
    // 給手一個tag（例如“Hand”）
    [SerializeField] private string handTag = "Hand";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("觸發 OnTriggerEnter，觸發物件的 Tag 為：" + other.tag);

        if (other.CompareTag(handTag))
        {
            Debug.Log("觸發成功，切換到場景 memory");
            SceneManager.LoadScene("memory");
        }
    }

}
