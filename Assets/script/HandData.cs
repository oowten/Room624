using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandData : MonoBehaviour
{
    //辨識左右手
    public enum HandModelType {Left, Right}
    public HandModelType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones;
}
