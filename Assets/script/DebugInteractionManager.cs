using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DebugInteractionManager : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    void Start()
    {
        interactionManager = FindObjectOfType<XRInteractionManager>();
        if (interactionManager != null)
        {
            Debug.Log("XR Interaction Manager found: " + interactionManager.name);
        }
        else
        {
            Debug.LogWarning("No XR Interaction Manager found in scene!");
        }
    }
}
