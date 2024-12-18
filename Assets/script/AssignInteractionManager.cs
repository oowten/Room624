using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AssignInteractionManager : MonoBehaviour
{
    private void Start()
    {
        XRInteractionManager interactionManager = FindObjectOfType<XRInteractionManager>();
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();

        if (interactionManager != null && grabInteractable != null)
        {
            grabInteractable.interactionManager = interactionManager;
        }
        else
        {
            Debug.LogWarning("XR Interaction Manager or XR Grab Interactable is missing!");
        }
    }
}