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
        float scale = GameplayCalculatorFunctions.FeetToUnityMeasurement(CastingAbilityManager.GetInstance().abilityToCastInformation.effects[0].areaOfEffect.range);
        gameObject.transform.localScale = new Vector3(2*scale, 0.00003f, 2*scale);
    }

    void SetPosition()
    {
        transform.SetParent(GameManager.GetInstance().playingCharacterGameObjects[CastingAbilityManager.GetInstance().attacker.name].transform);
        transform.localPosition = Vector3.zero;
        gameObject.transform.localPosition = new Vector3(0, 1f, 0);
    }

    public void Activate()
    {
        SetScale();
        SetPosition();
    }
}
