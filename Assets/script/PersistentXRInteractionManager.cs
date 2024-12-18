using UnityEngine;

public class PersistentXRInteractionManager : MonoBehaviour
{
    private static PersistentXRInteractionManager instance;

    void Awake()
    {
        // 確保只有一個 XR Interaction Manager 實例
        if (instance != null)
        {
            Destroy(gameObject); // 刪除重複的物件
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 保留物件在場景切換時不被銷毀
    }
}
