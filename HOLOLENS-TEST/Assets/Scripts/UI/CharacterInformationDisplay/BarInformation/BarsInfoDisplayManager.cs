using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsInfoDisplayManager : MonoBehaviour
{
    public GameObject BarInfoPrefab;
    
    List<GameObject> barsList = new List<GameObject>();

    public void CreateUI(Character displayingCharacter)
    {
        foreach (Character_UI_Bar bar in displayingCharacter.character_UI_info.bars.Values)
        {
            GameObject instance = Instantiate(BarInfoPrefab, gameObject.transform);
            BarInfo barInfo = instance.GetComponent<BarInfo>();

            barInfo.SetStatName(bar.statName);
            barInfo.SetValue(
                displayingCharacter.GetStat(bar.statName).GetCurrentValue(),
                displayingCharacter.GetStat(bar.statName).GetMaxValue()
            );

            barsList.Add(instance);
        }
    }

    public void DestroyUI()
    {
        foreach(GameObject bar in barsList)
        {
            Destroy(bar);
        }

        barsList.Clear();
    }
}
