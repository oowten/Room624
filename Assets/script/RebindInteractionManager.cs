using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RebindInteractionManager : MonoBehaviour
{
    private XRInteractionManager interactionManager;

    void Start()
    {
        // 獲取場景中唯一的 XR Interaction Manager
        interactionManager = FindObjectOfType<XRInteractionManager>();

        // 重新綁定 XR Grab Interactable
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.interactionManager = interactionManager;
            Debug.Log("Interaction Manager 已重新綁定給：" + gameObject.name);
        }

        // 如果有其他 XR Interactor 或其他邏輯也需要綁定，可以在此添加
    }
}
