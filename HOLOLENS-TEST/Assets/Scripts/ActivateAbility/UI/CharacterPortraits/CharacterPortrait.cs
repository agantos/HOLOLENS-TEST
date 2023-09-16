using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterPortrait : MonoBehaviour
{
    //Set in unity editor
    public TextMeshPro title;
    public MeshRenderer portraitImage;
    public BarInfo bar;

    string characterName;
    Character character;        

    string path = "Assets/Resources/UI/CharacterPortraits/Character Art/";

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
        title.text = name;

        //Set HP
        bar.SetValue(character.GetStat("HP").GetCurrentValue(), character.GetStat("HP").GetMaxValue());
        
        //Set Image
        portraitImage.material = new Material(Shader.Find("Transparent/Diffuse"));
        portraitImage.material.mainTexture = LoadTexture(path + characterName + ".png");
    }

    Texture2D LoadTexture(string path)
    {
        // Load the texture as a byte array
        byte[] fileData = System.IO.File.ReadAllBytes(path);

        // Create a new texture
        Texture2D texture = new Texture2D(2, 2);

        // Load the image data into the texture
        if (texture.LoadImage(fileData))
        {
            return texture;
        }

        return null;
    }
}
