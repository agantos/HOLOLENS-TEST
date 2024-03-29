using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OtherStatsManager : MonoBehaviour
{
    public GameObject OtherStatsInfoPrefab;

    List<GameObject> spawnedObjectsList = new List<GameObject>();
    public void CreateUI(Ability displayingAbility, Character character = null)
    {
        CreateDuration(displayingAbility);
        CreateRange(displayingAbility);
        CreateShape(displayingAbility);
        CreateTargets(displayingAbility);
    }

    void CreateStatInfo(string text, string imagePath)
    {
        GameObject element = Instantiate(OtherStatsInfoPrefab, gameObject.transform);
        element.GetComponentInChildren<RawImage>().texture = Resources.Load<Texture2D>(imagePath);
        element.GetComponentInChildren<TMP_Text>().text = text;

        spawnedObjectsList.Add(element);
    }

    void CreateRange(Ability displayingAbility)
    {
        AreaOfEffectStats abilityAOEInfo = displayingAbility.effects[0].areaOfEffect;

        int range = abilityAOEInfo.range;
        string text = range + " feet";

        CreateStatInfo(text, "UI/AbilityDisplayImages/Range/Range");
    }

    void CreateDuration(Ability displayingAbility)
    {
        int abilityDuration = displayingAbility.effects[0].duration;
        string text;
        
        if (abilityDuration == 0)
            text = "Instantaneous";
        else
            text = abilityDuration + "Rounds";

        CreateStatInfo(text, "UI/AbilityDisplayImages/Duration/Duration");
    }

    void CreateTargets(Ability displayingAbility)
    {
        TargettingStats abilityTargetting = displayingAbility.effects[0].targetting;

        string text = "";
        string path = "";

        switch (abilityTargetting.targetType)
        {
            case TargetType.SELF:
                text = "Self";
                path = "UI/AbilityDisplayImages/Target/Target_Self";
                break;

            case TargetType.ALLY:
                text = "Allies";
                path = "UI/AbilityDisplayImages/Target/Target_Allies";
                break;

            case TargetType.ENEMY:
                text = "Enemies";
                path = "UI/AbilityDisplayImages/Target/Target_Enemies";
                break;

            case TargetType.ALL:
                text = "Any Creature";
                path = "UI/AbilityDisplayImages/Target/Target_Any";
                break;

            case TargetType.ALL_NOT_SELF:
                Debug.Log("Target type: ALL_NOT_SELF");
                break;

            case TargetType.AREA:
                text = "Area";
                path = "UI/AbilityDisplayImages/Target/Target_Self";
                break;

            case TargetType.TYPED:
                Debug.Log("Target type: TYPED");
                break;

            default:
                Debug.Log("Unknown target type");
                break;
        }
        CreateStatInfo(text, path);
    }

    void CreateShape(Ability displayingAbility)
    {
        AreaOfEffectStats abilityAOEInfo = displayingAbility.effects[0].areaOfEffect;

        string text = "";
        string path = "";

        int radius = abilityAOEInfo.radius;

        switch (abilityAOEInfo.shape)
        {
            case AreaShape.CUBE:
                text = radius + "feet";
                path = "UI/AbilityDisplayImages/Shape/Cube";
                break;

            case AreaShape.CONE:
                text = radius + "feet";
                path = "UI/AbilityDisplayImages/Shape/Cone";
                break;

            case AreaShape.SPHERE:
                text = radius + "feet";
                path = "UI/AbilityDisplayImages/Shape/Sphere";
                break;

            case AreaShape.LINE:
                text = radius + "feet";
                path = "UI/AbilityDisplayImages/Shape/Line";
                break;

            case AreaShape.SELECT:
                text = "Select";
                path = "UI/AbilityDisplayImages/Shape/Select";
                break;

            case AreaShape.CIRCLE:
                text = radius + "feet";
                path = "UI/AbilityDisplayImages/Shape/Sphere";
                break;

            default:
                Debug.Log("Unknown area shape");
                break;
        }

        CreateStatInfo(text, path);
    }

    public void ClearUI() { 
        
        foreach(GameObject obj in spawnedObjectsList)
        {
            Destroy(obj);
        }

        spawnedObjectsList.Clear();
    }
}
