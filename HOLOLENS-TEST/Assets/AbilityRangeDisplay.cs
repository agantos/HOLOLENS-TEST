using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRangeDisplay : MonoBehaviour
{
    void SetScale()
    {
        float scale = GameplayCalculatorFunctions.FeetToUnityMeasurement(CastingAbilityManager.abilityToCast.effects[0].areaOfEffect.range);
        gameObject.transform.localScale = new Vector3(scale, 0.00003f, scale);
    }

    void SetPosition()
    {
        Vector3 attackerPosition = GameManager.characterGameObjects[CastingAbilityManager.attacker.name].transform.localPosition;
        float bottomOfAttacker = attackerPosition.y - GameManager.characterGameObjects[CastingAbilityManager.attacker.name].transform.localScale.y;
        gameObject.transform.localPosition = new Vector3(attackerPosition.x, bottomOfAttacker, attackerPosition.z);
    }

    public void Initialize()
    {
        SetScale();
        SetPosition();
    }
}
