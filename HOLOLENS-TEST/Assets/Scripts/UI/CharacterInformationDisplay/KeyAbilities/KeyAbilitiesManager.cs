using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAbilitiesManager : MonoBehaviour
{
    //Set in Editor
    public GameObject KeyAbilityPrefab;
    public GameObject KeyAbilityRowPrefab;

    List<GameObject> keyAbilitiesList = new List<GameObject>();
    List<GameObject> rowList = new List<GameObject>();
    GameObject currentRow;

    //Key abilities are created in rows of max 4 abilities
    public void CreateUI(Character displayingCharacter)
    {
        int counter = 0;
        foreach (string ability in displayingCharacter.character_UI_info.keyAbilities.Values)
        {
            // Create new row if needed
            if (counter % 4 == 0)
            {
                currentRow = Instantiate(KeyAbilityRowPrefab, gameObject.transform);
                rowList.Add(currentRow);
            }

            // Add KeyAbility to current row
            GameObject instance = Instantiate(KeyAbilityPrefab, currentRow.transform);
            instance.GetComponent<KeyAbilityInfo>().SetName(ability);
            instance.GetComponent<KeyAbilityInfo>().SetOnClick(delegate { FindAnyObjectByType<AbilityDisplayManager>().CreateUI(ability); });
            keyAbilitiesList.Add(instance);

            counter++;
        }
    }

    public void ClearUI()
    {
        foreach (GameObject obj in keyAbilitiesList)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in rowList)
        {
            Destroy(obj);
        }

        keyAbilitiesList.Clear();
        rowList.Clear();

    }
}
