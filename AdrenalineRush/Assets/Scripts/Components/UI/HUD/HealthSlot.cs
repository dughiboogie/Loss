using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSlot : MonoBehaviour
{
    private Animator animator;
    public bool isFull;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isFull = true;
    }

    public void Fill()
    {
        animator.SetTrigger("HealthUp");
        isFull = true;
    }

    public void Empty()
    {
        animator.SetTrigger("HealthDown");
        isFull = false;
    }

}
