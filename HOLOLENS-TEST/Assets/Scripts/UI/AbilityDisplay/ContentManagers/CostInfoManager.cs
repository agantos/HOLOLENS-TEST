using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CostDisplayTextRow{
    public List<GameObject> textList;
    public GameObject gameObject;

    public CostDisplayTextRow(GameObject g)
    {
        this.gameObject = g;
        textList = new List<GameObject>();
    }
}

public class CostInfoManager : MonoBehaviour
{
    //Pass through Unity Editor
    public GameObject TextPrefab;
    public GameObject TextRowPrefab;
    public GameObject TextGridInstance;

    List<CostDisplayTextRow> rowList = new List<CostDisplayTextRow>();

    public void CreateUI(Ability displayingAbility, Character character = null)
    {
        int count = 0;

        //Add TurnEconomyCosts
        foreach (string name in displayingAbility.turnEconomyCost.Keys)
        {
            //Each Row has 3 elements
            if (count % 3 == 0)
                CreateRow();

            string cost = displayingAbility.turnEconomyCost[name].ToString();
            
            string characterCurrent = "";
            if(character!= null)
            {
                characterCurrent = character.currentTurnEconomy[name].ToString();
            }

            //Create text of base cost
            string text = "";
            text += "(" + displayingAbility.turnEconomyCost[name].ToString() + ") ";
            text += AbilityDisplayGeneralMethods.Instance.ColorString(name, AbilityDisplayColors.TURN_ECONOMY);

            //If a character is passed create text for current turn economy
            if(character != null)
            {
                text += "\n Current: " + AbilityDisplayGeneralMethods.Instance.ColorString(characterCurrent, AbilityDisplayColors.TURN_ECONOMY);
            }

            AddToCurrentRow(text);

            count++;
        }

        //Add Stat Costs
        foreach (string name in displayingAbility.statCost.Keys)
        {
            //Each Row has 3 elements
            if (count % 3 == 0)
                CreateRow();

            //Create Text and add to current row
            string text = "";
            text += "(" + displayingAbility.statCost[name].ToString() + ") ";
            text += AbilityDisplayGeneralMethods.Instance.ColorString(name, AbilityDisplayColors.CHARACTER_STAT);
            AddToCurrentRow(text);

            count++;
        }
    }

    void CreateRow()
    {
        GameObject instance = Instantiate(TextRowPrefab, TextGridInstance.transform);
        rowList.Add(new CostDisplayTextRow(instance));
    }

    void AddToCurrentRow(string text) {
        CostDisplayTextRow rowToAdd = rowList.Last();
        GameObject textElement = Instantiate(TextPrefab, rowToAdd.gameObject.transform);

        textElement.GetComponent<TMP_Text>().text = text;
    }

    public void ClearUI()
    {
        foreach(CostDisplayTextRow row in rowList)
        {
            Destroy(row.gameObject);
            row.textList.Clear();
        }
        rowList.Clear();
    }
}
