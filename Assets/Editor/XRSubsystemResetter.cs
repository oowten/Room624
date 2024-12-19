using UnityEngine;
using UnityEditor;
using UnityEngine.XR.Management;

[InitializeOnLoad]
public class XRSubsystemResetter
{
    static XRSubsystemResetter()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("Resetting XR Subsystem after exiting Play Mode...");
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            Debug.Log("Initializing XR Subsystem when entering Play Mode...");
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

            // 強制重新綁定 XR Interaction Manager
            RebindAllInteractionManagers();
        }
    }

    private static void RebindAllInteractionManagers()
    {
        Debug.Log("Rebinding XR Interaction Managers...");
        XRInteractionManager[] managers = Object.FindObjectsOfType<XRInteractionManager>();
        foreach (var manager in managers)
        {
            Debug.Log("Found Interaction Manager: " + manager.name);
            // 如果需要，可以在這裡添加其他初始化邏輯
        }
    }

}


