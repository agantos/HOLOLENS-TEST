using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

[System.Serializable]
public class JSONAbilityAnimationTypes
{
    public string attacker;
    public string defender_AbilitySucceeds;
    public string defender_AbilityFails;

    AnimationType StringToAnimationType(string s)
    {
        AnimationType type;

        if (Enum.TryParse(s, out type) == false)
            Assert.IsFalse(true);

        return type;
    }

    public AnimationType GetAttackerAnimationType()
    {
        return StringToAnimationType(attacker);
    }

    public AnimationType GetDefender_AbilitySucceeds()
    {
        return StringToAnimationType(defender_AbilitySucceeds);
    }

    public AnimationType GetDefender_AbilityFails()
    {
        return StringToAnimationType(defender_AbilityFails);
    }
}

[System.Serializable]
public class JSONVisualEffects
{
    public string onImpact;
    public string duringActivation;
}

[System.Serializable]
public class JSONSoundEffects
{
    public string onImpact;
    public string duringActivation;
}

[System.Serializable]
public class JSONCharacterDialogue
{
    public string onImpact;
    public string duringActivation;
    public string defender_AbilityFails;
    public string defender_AbilitySucceeds;
}

[System.Serializable]
public class JSONAbilityPresentation
{
    public string abilityName;
    public JSONAbilityAnimationTypes animations;
    public JSONVisualEffects visualEffects;
    public JSONSoundEffects soundEffects;
    public JSONCharacterDialogue characterDialogue;
}

[System.Serializable]
public class JSONAbilityPresentations
{
    public JSONAbilityPresentation[] abilitiesPresentation;    
}
