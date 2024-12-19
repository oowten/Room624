using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchOnTouchToMemory : MonoBehaviour
{
    // 給手一個tag（例如“Hand”）
    [SerializeField] private string handTag = "Hand";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(handTag))
        {
            // 切換到目標場景
            SceneManager.LoadScene("memory");
        }
    }
}