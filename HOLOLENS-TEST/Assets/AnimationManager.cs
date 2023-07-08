using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    int AnimatorParameter_IsMoving;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>(true);
        AnimatorParameter_IsMoving = Animator.StringToHash("IsMoving");
    }

    public bool GetHasMovingAnimation()
    {
        return animator.GetBool(AnimatorParameter_IsMoving);
    }

    public void IdleToMoving()
    {
        animator.SetBool(AnimatorParameter_IsMoving, true);
    }

    public void MovingToIdle()
    {
        animator.SetBool(AnimatorParameter_IsMoving, false);
    }
}
