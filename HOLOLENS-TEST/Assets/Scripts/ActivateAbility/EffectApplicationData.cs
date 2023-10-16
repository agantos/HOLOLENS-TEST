using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AbilitySelectType { SHAPE, SELF_SHAPE, SELECT, INACTIVE }

[System.Serializable]
public class EffectApplicationData
{
    public EffectType type;

    public int duration;
    public int damage;

    public string affectedStat;
    public bool effectSucceeds;

    public string attacker;
    public string defender;
    public string abilityName;

    public EffectApplicationData(EffectType type, int duration, string abilityName,
                                 int damage, string statAffected,
                                 bool effectSucceeds, string attacker, string defender)
    {
        this.type = type;
        this.duration = duration;
        this.damage = damage;
        this.affectedStat = statAffected;
        this.abilityName = abilityName;
        this.effectSucceeds = effectSucceeds;
        this.attacker = attacker;
        this.defender = defender;
    }

    public void ApplyEffect()
    {
        Character defenderCharacter = GameManager.GetInstance().playingCharacterPool[defender];
        GameObject defenderGameObject = GameManager.GetInstance().playingCharacterGameObjects[defender];
        CharacterStat stat = defenderCharacter.GetStat(affectedStat);

        switch (type)
        {
            case EffectType.DAMAGE:
                //Deal damage
                stat.DealDamage(damage);

                //Spawn Floating Text
                GameObject text = GameObject.Instantiate(GameManager.GetInstance().FloatingTextPrefab, defenderGameObject.transform);
                text.GetComponent<FloatingText>().Inititalize(damage.ToString(), true);

                break;
            case EffectType.HEALING:
                stat.HealDamage(damage);
                break;
            case EffectType.TEMPORAL:
                if (effectSucceeds)
                    defenderCharacter.GetStats().AddTemporalEffect(affectedStat, "", duration, damage);
                break;
        }

        stat.CalculateCurrentValue();
    }

    public void LogApplication()
    {
        Character defenderCharacter = GameManager.GetInstance().playingCharacterPool[defender];
        Character attackerCharacter = GameManager.GetInstance().playingCharacterPool[attacker];

        switch (type)
        {
            case EffectType.DAMAGE:
                Logger.Instance.Log_Damage((int)damage, affectedStat, defenderCharacter, attackerCharacter);
                break;
            case EffectType.HEALING:
                Logger.Instance.Log_Heal((int)damage, affectedStat, defenderCharacter, attackerCharacter);
                break;
            case EffectType.TEMPORAL:
                Logger.Instance.Log_Apply_Temporal((int)damage, affectedStat, duration, defenderCharacter, attackerCharacter);
                break;
        }
    }

    // Serialize the list to a JSON string
    public string SerializeListToJson()
    {
        return JsonUtility.ToJson(this);
    }

    // Deserialize the JSON string back to a list
    public static EffectApplicationData DeserializeJsonToList(string jsonString)
    {
        return JsonUtility.FromJson<EffectApplicationData>(jsonString);
    }

    public override string ToString()
    {
        return $"Effect Type: {type}\n" +
               $"Duration: {duration}\n" +
               $"Damage: {damage}\n" +
               $"Affected Stat: {affectedStat}\n" +
               $"Effect Succeeds: {effectSucceeds}\n" +
               $"Attacker: {attacker}\n" +
               $"Defender: {defender}\n" +
               $"Ability Name: {abilityName}";
    }
}
