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
        // 找到當前場景的“RespawnPoint”
        GameObject respawnPoint = GameObject.Find("RespawnPoint");
        if (respawnPoint != null)
        {
            Transform xrRigTransform = GameObject.Find("XR Rig").transform;
            xrRigTransform.position = respawnPoint.transform.position;
            xrRigTransform.rotation = respawnPoint.transform.rotation;
        }
        else
        {
            Debug.LogWarning("RespawnPoint not found in the scene!");
        }
    }
}
