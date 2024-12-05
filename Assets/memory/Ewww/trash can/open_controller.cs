using UnityEngine;

public class Open_Controller : MonoBehaviour
{
    private Animator animator;
    private bool hasOpened = false; // �O���O�_�wĲ�o�L�ʵe

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !hasOpened)
        {
            hasOpened = true; // �аO�ʵe�wĲ�o
            animator.SetBool("isOpen", true); // Ĳ�o�ʵe����
        }
    }
}
