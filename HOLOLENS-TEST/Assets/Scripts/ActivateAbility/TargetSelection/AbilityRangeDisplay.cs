using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityRangeDisplay : MonoBehaviour
{
    private static AbilityRangeDisplay instance;
    public static AbilityRangeDisplay Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // Ensures only one instance exists
            return;
        }
        instance = this;
    }

    public void Deactivate()
    {
        gameObject.transform.localScale = new Vector3(0, 0.00003f, 0);
    }

    void SetScale()
    {
        float scale = GameplayCalculatorFunctions.FeetToUnityMeasurement(CastingAbilityManager.abilityToCast.effects[0].areaOfEffect.range);
        gameObject.transform.localScale = new Vector3(2*scale, 0.00003f, 2*scale);
    }

    void SetPosition()
    {
        Vector3 attackerPosition = GameManager.playingCharacterGameObjects[CastingAbilityManager.attacker.name].transform.localPosition;
        float bottomOfAttacker = attackerPosition.y - GameManager.playingCharacterGameObjects[CastingAbilityManager.attacker.name].transform.localScale.y;
        gameObject.transform.localPosition = new Vector3(attackerPosition.x, bottomOfAttacker + 1f, attackerPosition.z);
    }

    public void Activate()
    {
        SetScale();
        SetPosition();
    }
}
