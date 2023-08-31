using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatsManager : MonoBehaviour
{
    //Set in Editor
    public GameObject BaseStatPrefab;
    public GameObject BaseStatsRowPrefab;
    
    List<GameObject> baseStatsList = new List<GameObject>();
    List<GameObject> containersList = new List<GameObject>();
    GameObject currentRow;

    //Base stats are created in rows of max 5 stats
    //Each Container is a row
    public void CreateUI(Character displayingCharacter)
    {
        int counter = 0;
        foreach(string stat in displayingCharacter.character_UI_info.baseStats.Values)
        {
            // Create new row if needed
            if(counter % 5 == 0)
            {
                currentRow = Instantiate(BaseStatsRowPrefab, gameObject.transform);
                containersList.Add(currentRow);
            }

            // Add Stat to current row
            GameObject instance = Instantiate(BaseStatPrefab, currentRow.transform);
            instance.GetComponent<BaseStat>().SetStatName(stat);
            instance.GetComponent<BaseStat>().SetValue(displayingCharacter.GetStat(stat).GetCurrentValue());

            counter++;
        }
    }
}
