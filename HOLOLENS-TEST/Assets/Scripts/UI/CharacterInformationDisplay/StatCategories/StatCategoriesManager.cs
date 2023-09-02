using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatCategoriesManager : MonoBehaviour
{
    public GameObject StatCategoryPrefab;
    public GameObject SectionTitlePrefab;
    public GameObject StatContainerPrefab;
    public GameObject StatPrefab;

    List<GameObject> statCategoryList = new List<GameObject>(),
                        statContainerList = new List<GameObject>(),
                        statList = new List<GameObject>(),
                        titleList = new List<GameObject>();
    public void CreateUI(Character displayingCharacter)
    {
        foreach (Character_UI_StatCategory category in displayingCharacter.character_UI_info.statCategories.Values)
        {
            //Create StatCategory
            GameObject statCategory = Instantiate(StatCategoryPrefab, transform);
            statCategoryList.Add(statCategory);

            //Create Section Title
            GameObject title = Instantiate(SectionTitlePrefab, statCategory.transform);
            title.GetComponent<CharacterUI_SectionTitle>().SetTitle(category.categoryName);
            titleList.Add(title);

            //Create StatContainer
            GameObject statContainer = Instantiate(StatContainerPrefab, statCategory.transform);
            statContainerList.Add(statContainer);

            //Populate StatContainer
            foreach (string stat in category.statNames)
            {
                GameObject statInstance = Instantiate(StatPrefab, statContainer.transform);
                statInstance.GetComponent<UI_Stat>().SetStatName(stat);
                statInstance.GetComponent<UI_Stat>().SetValue(displayingCharacter.GetStat(stat).GetCurrentValue());

                statList.Add(statInstance);
            }
        }

        CreateAllStatUI(displayingCharacter);
    }

    public void CreateAllStatUI(Character displayingCharacter)
    {
        //Create StatCategory
        GameObject statCategory = Instantiate(StatCategoryPrefab, transform);
        statCategoryList.Add(statCategory);

        //Create Section Title
        GameObject title = Instantiate(SectionTitlePrefab, statCategory.transform);
        title.GetComponent<CharacterUI_SectionTitle>().SetTitle("All Stats");
        titleList.Add(title);

        //Create StatContainer
        GameObject statContainer = Instantiate(StatContainerPrefab, statCategory.transform);
        statContainerList.Add(statContainer);

        //Populate StatContainer
        foreach (string stat in displayingCharacter.GetStats().GetStatistics().Keys)
        {
            GameObject statInstance = Instantiate(StatPrefab, statContainer.transform);
            statInstance.GetComponent<UI_Stat>().SetStatName(stat);
            statInstance.GetComponent<UI_Stat>().SetValue(displayingCharacter.GetStat(stat).GetCurrentValue());

            statList.Add(statInstance);
        }
    }

    public void ClearUI()
    {
        foreach (GameObject obj in statList)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in statContainerList)
        {
            Destroy(obj);
        }
        foreach (GameObject obj in titleList)
        {
            Destroy(obj);
        }

        foreach (GameObject obj in statCategoryList)
        {
            Destroy(obj);
        }
    }
}
