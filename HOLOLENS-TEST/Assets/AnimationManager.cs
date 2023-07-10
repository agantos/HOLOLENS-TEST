using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    int AnimatorParameter_IsMoving;
    int AnimatorParameter_MeleeAttacks_Sword_OneHanded;
    // Start is called before the first frame update
    void Start()
    {        
        animator = GetComponentInChildren<Animator>(true);
        AnimatorParameter_IsMoving = Animator.StringToHash("IsMoving");
        AnimatorParameter_MeleeAttacks_Sword_OneHanded = Animator.StringToHash("MeleeAttacks_Sword_OneHanded");
    }

    //Miscellaneous
    public float GetCurrentAnimationDuration()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    //Idle ---> OtherState

    public void IdleTo_Moving()
    {
        animator.SetBool(AnimatorParameter_IsMoving, true);
    }

    public void IdleTo_MeleeAttacking_Sword_OneHanded()
    {
        int animation_variations = 3; //number of different animations for the same attack
        animator.SetInteger(AnimatorParameter_MeleeAttacks_Sword_OneHanded, GameplayCalculatorFunctions.random.Next(1, animation_variations+1));
    }

    //State ----> Idle
    public void MovingTo_Idle()
    {
        animator.SetBool(AnimatorParameter_IsMoving, false);
        int animation_length = animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public void MeleeAttacking_Sword_OneHandedTo_Idle()
    {
        animator.SetInteger(AnimatorParameter_MeleeAttacks_Sword_OneHanded, 0);
    }


    //Getters
    public bool GetHasMovingAnimation()
    {
        return animator.GetBool(AnimatorParameter_IsMoving);
    }

}
