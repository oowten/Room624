using UnityEngine;
using UnityEngine.SceneManagement;

public class XRRigPositionManager : MonoBehaviour
{
    private static XRRigPositionManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 找到場景中的RespawnPoint
        GameObject respawnPoint = GameObject.Find("RespawnPoint");

        // 確保只有一個xr rig
        XRRigPositionManager existingRig = FindObjectOfType<XRRigPositionManager>();

        if (respawnPoint != null && existingRig != null)
        {
            Transform xrRigTransform = existingRig.transform;
            xrRigTransform.position = respawnPoint.transform.position;
            xrRigTransform.rotation = respawnPoint.transform.rotation;

            Debug.Log("XR Rig 重置到新場景的 RespawnPoint 位置");
        }
        else
        {
            Debug.LogWarning("RespawnPoint or XR Rig not found in the scene!");
        }
    }

}
