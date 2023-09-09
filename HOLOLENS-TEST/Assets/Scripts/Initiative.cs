using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FloatDescendingComparer : IComparer<float>
{
    public int Compare(float x, float y)
    {
        // Compare in descending order
        return y.CompareTo(x);
    }
}

public class TurnManager
{
    SortedDictionary<float, string> initiativeOrder = new SortedDictionary<float, string>(new FloatDescendingComparer());

    float currentInitiative;

    public TurnManager(Dictionary<string, Character> characterPool)
    {
        InitializeInitiativeOrder(characterPool);
        Debug.Log(ToString());
    }

    public void InitializeInitiativeOrder(Dictionary<string, Character> characterPool)
    {
        foreach (Character c in characterPool.Values)
        {
            AddToDictionary(CalculatePosition(c.GetStats()), c);
        }

        var enumerator = initiativeOrder.Keys.GetEnumerator();
        enumerator.MoveNext();
        currentInitiative = enumerator.Current;
    }
    public string GetCurrentTurn_Name()
    {
        return initiativeOrder[currentInitiative];
    }

    public void AdvanceInitiative()
    {
        //The first initiative smaller than the currentInitative is the next initiative
        foreach (float initiative in initiativeOrder.Keys)
        {
            if (initiative < currentInitiative)
            {
                currentInitiative = initiative;
                return;
            }
        }

        //If no such element exists start from the beggining.
        var enumerator = initiativeOrder.Keys.GetEnumerator();
        enumerator.MoveNext();
        currentInitiative = enumerator.Current;
    }

    void AddToDictionary(float initiative, Character character)
    {
        if (initiativeOrder.ContainsKey(initiative))
        {
            float newPosition = SolveTie(character, GameManager.GetInstance().playingCharacterPool[initiativeOrder[initiative]], initiative);
            AddToDictionary(newPosition, character);
        }
        else
        {
            initiativeOrder[initiative] = character.name;
        }
    }

    //Change according to ruleset
    float CalculatePosition(CharacterStats charStats)
    {
        return GameplayCalculatorFunctions.CalculateDiceRoll("1d20");
    }

    //Change according to ruleset
    //Return the position of the entering character.
    float SolveTie(Character entering, Character existing, float position)
    {
        return position + 0.01f;
    }

    public void GiveTurnToCharacter(string name)
    {
        Debug.Log("It's " + name + "'s turn");

        //Refresh character as needed at the start of the turn
        GameManager.GetInstance().playingCharacterPool[name].OnStartTurn();      

        //Change UI to play the selected character
        
    }

    public void NextTurn()
    {
        AdvanceInitiative();
        GiveTurnToCharacter(GetCurrentTurn_Name());
    }

    public void FirstTurn()
    {
        GiveTurnToCharacter(GetCurrentTurn_Name());
    }

    public string ToString()
    {
        string s = "";
        foreach (float position in initiativeOrder.Keys)
        {
            s += position.ToString() + ": " + initiativeOrder[position] + "\n";
        }
        return s;
    }
}
