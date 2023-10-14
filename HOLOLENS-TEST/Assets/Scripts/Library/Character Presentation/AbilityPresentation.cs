using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffects
{
    public string onImpact;
    public string duringActivation;

    public VisualEffects(string onImpact, string duringActivation)
    {
        this.onImpact = onImpact;
        this.duringActivation = duringActivation;
    }
}

public class SoundEffects
{
    public string onImpact;
    public string duringActivation;

    public SoundEffects(string onImpact, string duringActivation)
    {
        this.onImpact = onImpact;
        this.duringActivation = duringActivation;
    }
}

public class CharacterDialogue
{
    public string onImpact;
    public string duringActivation;
    public string defender_AbilityFails;
    public string defender_AbilitySucceeds;

    public CharacterDialogue(string onImpact, string duringActivation, string def_fail, string def_succ)
    {
        this.onImpact = onImpact;
        this.duringActivation = duringActivation;
        defender_AbilityFails = def_fail;
        defender_AbilitySucceeds = def_succ;
    }
}

public class Animations
{
    public AnimationType attacker;
    public AnimationType defender_AbilitySucceeds;
    public AnimationType defender_AbilityFails;

    public Animations(AnimationType att, AnimationType def_succ, AnimationType def_fail)
    {
        attacker = att;
        defender_AbilityFails = def_fail;
        defender_AbilitySucceeds = def_succ;
    }
}

public class AbilityPresentation
{
    public Animations animations;
    public VisualEffects visualEffects;
    public SoundEffects soundEffects;
    public CharacterDialogue characterDialogue;

    public AbilityPresentation(JSONAbilityPresentation abilityPresentation)
    {
        animations = new Animations(
            abilityPresentation.animations.GetAttackerAnimationType(), 
            abilityPresentation.animations.GetDefender_AbilitySucceeds(),
            abilityPresentation.animations.GetDefender_AbilityFails()            
        );

        visualEffects = new VisualEffects(
            abilityPresentation.visualEffects.onImpact, 
            abilityPresentation.visualEffects.duringActivation
        );

        soundEffects = new SoundEffects(
            abilityPresentation.soundEffects.onImpact,
            abilityPresentation.soundEffects.duringActivation
        );

        characterDialogue = new CharacterDialogue(
            abilityPresentation.characterDialogue.onImpact,
            abilityPresentation.characterDialogue.duringActivation,
            abilityPresentation.characterDialogue.defender_AbilityFails,
            abilityPresentation.characterDialogue.defender_AbilitySucceeds
        );
    }
}
