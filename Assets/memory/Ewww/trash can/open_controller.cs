using UnityEngine;

public class Open_Controller : MonoBehaviour
{
    private Animator animator;
    private bool hasOpened = false; // 記錄是否已觸發過動畫

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !hasOpened)
        {
            hasOpened = true; // 標記動畫已觸發
            animator.SetBool("isOpen", true); // 觸發動畫播放
        }
    }
}
