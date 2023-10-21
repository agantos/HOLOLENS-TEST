using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Microsoft.MixedReality.Toolkit.UI;

public class CharacterPortrait : MonoBehaviour
{
    //Set in unity editor
    public TextMeshPro frontTitle, backTitle;
    public MeshRenderer portraitImage;
    public BarInfo bar;
    public Interactable button;

    string characterName;
    Character character;        

    string path = "UI/CharacterPortraits/Character Art/";

    private void Update()
    {
        //Set HP
        bar.SetValue(character.GetStat("HP").GetCurrentValue(), character.GetStat("HP").GetMaxValue());
    }

    public void Initialize(string name)
    {
        //Set name and character
        characterName = name;
        character = GameManager.GetInstance().playingCharacterPool[characterName];
        
        frontTitle.text = name;
        backTitle.text = name;

        //Set HP
        bar.SetValue(character.GetStat("HP").GetCurrentValue(), character.GetStat("HP").GetMaxValue());
        
        //Set Image
        portraitImage.material = new Material(Shader.Find("UI/Unlit/Transparent"));
        portraitImage.material.mainTexture = Resources.Load<Texture2D>(path + name);

        //Set OnClick
        button.OnClick.AddListener(OnClick);
    }

    void OnClick()
    {
        GameObject cGameObject = GameManager.GetInstance().playingCharacterGameObjects[characterName];
        cGameObject.GetComponent<SelectUnitManager>().OnTouchCompleted(null);
    }

}
