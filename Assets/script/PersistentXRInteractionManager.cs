using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PersistentXRInteractionManager : MonoBehaviour
{
    private static PersistentXRInteractionManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 如果已存在，刪除重複的物件
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 保留 Interaction Manager
    }
}
