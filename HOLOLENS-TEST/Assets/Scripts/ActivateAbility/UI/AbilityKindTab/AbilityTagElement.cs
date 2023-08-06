using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityTagElement : MonoBehaviour
{

    public GameObject buttonGameObject;
    public TMP_Text nameText;
    public string abilityTag;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNameGameobject(string name)
    {
        nameText.SetText(name);
    }

    public void SetButtonOnClick(UnityEngine.Events.UnityAction method)
    {
        buttonGameObject.GetComponent<Button>().onClick.AddListener(method);
    }
}
