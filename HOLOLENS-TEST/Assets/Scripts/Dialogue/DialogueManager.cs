using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class DialogueManager : MonoBehaviour
{
    AudioSource currentPlayingSource = null;
    public static DialogueManager Instance { get; private set; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDefenderDialogue(List<EffectApplicationData> applicationData)
    {
        string dialogueType, character;
        List<EffectApplicationData> primaryEffectsApplicationData = new List<EffectApplicationData>();

        //Find all the appropriate reactions for the main effects that are applied for all defenders
        foreach (EffectApplicationData appDatum in applicationData)
        {
            primaryEffectsApplicationData.Add(appDatum);
        }

        //Select one of them to play
        int selected = GameplayCalculatorFunctions.random.Next(primaryEffectsApplicationData.Count);
        
        GetDefenderDialogueType(primaryEffectsApplicationData.ElementAt(selected), out dialogueType, out character);

        //Play it
        PlayDialogue(character, dialogueType, 100);
    }

    public void PlayDuringActivationDialogue(string charName, AbilityPresentation abilityPresentation)
    {
        PlayDialogue(charName, abilityPresentation.characterDialogue.duringActivation, 100);
    }

    void GetDefenderDialogueType(EffectApplicationData effect, out string dialogueType, out string character)
    {
        character = effect.defender;

        Debug.Log(effect.type.ToString());

        if (effect.type == EffectType.DAMAGE)
        {
            if (effect.effectSucceeds)
            {
                dialogueType = "onGettingHit";
                return;
            }
            else {
                dialogueType = "onDodge";
                return;
            }
        }
        else if(effect.type == EffectType.HEALING)
        {
            dialogueType = "onGettingHealed";
            return;
        }
        else if(effect.type == EffectType.TEMPORAL)
        {
            if (!effect.effectSucceeds)
            {
                dialogueType = "onDodge";
                return;
            }
            else
            {
                //Buff
                if (effect.damage < 0)
                {
                    dialogueType = "onGettingDebuffed";
                    return;
                }
                //Debuff
                else if (effect.damage > 0)
                {
                    dialogueType = "onGettingBuffed";
                    return;
                }
            }
            
        }

        //Shouldn't get here
        dialogueType = "";
    }

    public void PlayDialogue(string charName, string category, int percentageToPlay)
    {
        AudioSource prevAudioSource = currentPlayingSource;

        //Set the new playing source
        currentPlayingSource = GameManager.GetInstance().
                                playingCharacterGameObjects[charName].
                                GetComponent<AudioSource>()
        ;

        currentPlayingSource.clip = GameManager.GetInstance()
                                               .playingCharacterGameObjects[charName]
                                               .GetComponent<CharacterScript>()
                                               .dialogue.GetClip(category)
        ;

        //Play the sound if the percentage is correct
        int rolled = GameplayCalculatorFunctions.random.Next(1,101);

        if (rolled <= percentageToPlay)
        {
            //Stop the previous source from playing
            if (prevAudioSource)
                prevAudioSource.Stop();

            currentPlayingSource.Play();
        }            
    }

}
