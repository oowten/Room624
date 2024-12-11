using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionManager : MonoBehaviour
{
    public Transform xrRig; // XR Rig��Transform�ޥ�

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �M�����I
        GameObject spawnPoint = GameObject.Find("PlayerSpawnPoint");
        if (spawnPoint != null && xrRig != null)
        {
            xrRig.position = spawnPoint.transform.position;
            xrRig.rotation = spawnPoint.transform.rotation;
        }
    }
}
