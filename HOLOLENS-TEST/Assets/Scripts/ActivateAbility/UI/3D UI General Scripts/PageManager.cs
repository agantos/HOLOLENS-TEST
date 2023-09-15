using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class PageManager : MonoBehaviour
{
    public GameObject PagePrefab;
    ArrayList pageInstances = new ArrayList();
    public int currentPageNumber;

    //Switch Content
    public void BeginAtFirstPage()
    {
        currentPageNumber = 0;
        for (int i = 0; i < pageInstances.Count; i++)
        {
            if (currentPageNumber == 0)
                ShowPage(0);
            else
                HidePage(i);
        }
    }

    public void NextPage()
    {
        //First Page
        if(currentPageNumber == 0)
        {
            //Spawn prev button
        }
        //Prev to last page
        else if(currentPageNumber == pageInstances.Count - 2)
        {
            //Despawn next button
        }

        currentPageNumber++;
        DisplayCurrentPage(currentPageNumber);
    }  

    public void PreviousPage()
    {
        //First Page
        if (currentPageNumber == 0)
        {
            //Spawn prev button
        }
        //Prev to last page
        else if (currentPageNumber == pageInstances.Count - 2)
        {
            //Despawn next button
        }

        currentPageNumber--;
        DisplayCurrentPage(currentPageNumber);
    }

    //Create Content
    public GameObject AddElement(GameObject Prefab)
    {
        return GetPageToAdd().GetComponent<PageScript>().AddElement(Prefab);
    }
    
    void CreatePage()
    {
        GameObject page = Instantiate(PagePrefab, transform);
        pageInstances.Add(page);
    }

    GameObject GetPageToAdd()
    {
        //First Page
        if (pageInstances.Count == 0){
            CreatePage();
        }
        //If the last page is full create another
        else if (((GameObject)pageInstances[pageInstances.Count-1]).GetComponent<PageScript>().IsFull())
        {
            CreatePage();
        }

        return ((GameObject)pageInstances[pageInstances.Count - 1]);
    }

    //Page Displaying
    void DisplayCurrentPage(int pageNumber)
    {
        ShowPage(pageNumber);

        if (pageNumber > 0)
            HidePage(pageNumber - 1);

        if (pageNumber < pageInstances.Count - 1)
            ShowPage(pageNumber + 1);
    }

    void ShowPage(int number)
    {
        ((GameObject)pageInstances[number]).SetActive(true);
    }

    void HidePage(int number)
    {
        ((GameObject)pageInstances[number]).SetActive(false);
    }

    public void ClearState()
    {
        foreach(GameObject pageInstance in pageInstances)
        {
            pageInstance.GetComponent<PageScript>().ClearState();
            Destroy(pageInstance);
        }

        pageInstances.Clear();
    }
}
