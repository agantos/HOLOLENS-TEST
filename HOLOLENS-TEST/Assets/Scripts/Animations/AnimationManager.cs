using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Attack_Melee_Weapon,
    Attack_Melee_Spell,
    Attack_Ranged_Weapon,
    Attack_Ranged_Spell,
    Spell_Cast_Area,
    Spell_Cast_Self,
    Spell_Cast_General,
    TakeHit,
    Dodge,
    Block,
    BreakEffect
}

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    int AnimatorParameter_IsMoving;
    int AnimatorParameter_Attack_Melee_Weapon;

    private Dictionary<AnimationType, (int hash, int variations)> animatorParameters = new Dictionary<AnimationType, (int hash, int variations)>();

    // Start is called before the first frame update
    void Start()
    {        
        animator = GetComponentInChildren<Animator>(true);
        AnimatorParameter_IsMoving = Animator.StringToHash("IsMoving");

        AddAnimatorParameter(AnimationType.Attack_Melee_Weapon, 3);
        AddAnimatorParameter(AnimationType.Attack_Melee_Spell, 1);
        AddAnimatorParameter(AnimationType.Attack_Ranged_Weapon, 1);
        AddAnimatorParameter(AnimationType.Attack_Ranged_Spell, 1);
        AddAnimatorParameter(AnimationType.Spell_Cast_Area, 1);
        AddAnimatorParameter(AnimationType.Spell_Cast_Self, 1);
        AddAnimatorParameter(AnimationType.Spell_Cast_General, 1);
        AddAnimatorParameter(AnimationType.TakeHit, 2);
        AddAnimatorParameter(AnimationType.Dodge, 2);
        AddAnimatorParameter(AnimationType.Block, 1);
        AddAnimatorParameter(AnimationType.BreakEffect, 1);
    }

    

    //Miscellaneous
    public float GetCurrentAnimationDuration()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void AddAnimatorParameter(AnimationType parameter, int variations)
    {
        int parameterHash = Animator.StringToHash(parameter.ToString());
        animatorParameters.Add(parameter, (parameterHash, variations));
    }

    //Idle ---> OtherState

    public void IdleTo_Moving()
    {
        animator.SetBool(AnimatorParameter_IsMoving, true);
    }

    public void IdleTo_Animation(AnimationType type)
    {
        animator.SetInteger(GetAnimatorParameter(type), GameplayCalculatorFunctions.random.Next(1, GetAnimationVariations(type) + 1));
    }

    //State ----> Idle
    public void MovingTo_Idle()
    {
        animator.SetBool(AnimatorParameter_IsMoving, false);
        int animation_length = animator.GetCurrentAnimatorClipInfo(0).Length;
    }

    public void Animation_ToIdle(AnimationType type)
    {
        animator.SetInteger(GetAnimatorParameter(type), 0);
    }


    //Getters
    public bool GetHasMovingAnimation()
    {
        return animator.GetBool(AnimatorParameter_IsMoving);
    }

    public int GetAnimatorParameter(AnimationType type)
    {
        return animatorParameters[type].hash;
    }

    public int GetAnimationVariations(AnimationType type)
    {
        return animatorParameters[type].variations;
    }

}
