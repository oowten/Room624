using UnityEngine;
using UnityEngine.InputSystem;

public class DebugInput : MonoBehaviour
{
    public InputActionProperty selectAction;

    void Update()
    {
        if (selectAction.action.ReadValue<float>() > 0)
        {
            Debug.Log("Select Action Triggered");
        }
    }
}
