using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class DialogueCategory
{
    public string name, tag;
    public List<AudioClip> clips;
    public int importance;

    public DialogueCategory(string name, int importance)
    {
        clips = new List<AudioClip>();
        this.name = name;
        this.importance = importance;
    }
}

public class CharacterDialogue
{
    Dictionary<string, DialogueCategory> dialogues = new Dictionary<string, DialogueCategory>();
    string path;

    public CharacterDialogue(string characterName)
    {
        path = "CharacterDialogue/" + characterName + "/";
    } 

    public void AddDialogueCategory(DialogueCategory dialogue)
    {
        dialogues.Add(dialogue.name, dialogue);
    }

    public DialogueCategory GetDialogueCategory(string name)
    {
        return dialogues[name];
    }

    public void CreateCategoryFromFolder(string folderName, string categoryName, int importance)
    {
        string folderPath = path + folderName;
        AudioClip[] array = Resources.LoadAll<AudioClip>(folderPath);

        if (array.Length > 0)
        {
            DialogueCategory newCategory = new DialogueCategory(categoryName, importance);


            foreach (AudioClip clip in array)
            {
                newCategory.clips.Add(clip);
            }

            AddDialogueCategory(newCategory);
        }
    }

    public AudioClip GetClip(string category)
    {
        if (dialogues.ContainsKey(category))
        {
            int clipNo = GameplayCalculatorFunctions.random.Next(dialogues[category].clips.Count);

            return dialogues[category].clips.ElementAt(clipNo);
        }
        else
            return null;
        
    }

}