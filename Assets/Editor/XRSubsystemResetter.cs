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
        }
    }
}


