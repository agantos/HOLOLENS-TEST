using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class StatTemporalEffect
{
    public int value, duration;
    public string name;

    public StatTemporalEffect(string name, int value, int duration)
    {
        this.value = value;
        this.name = name;
        this.duration = duration;
    }
}
public class StatFunctor
{
    public string op;
    public float value;

    public StatFunctor(string type, float operand)
    {
        this.op = type;
        this.value = operand;
    }
}
public class CharacterStatRelation
{
    public Character character;
    public string statName;
    public StatFunctor fun;

    public CharacterStatRelation(Character c,  string stat,  StatFunctor fun)
    {
        this.statName = stat;
        this.fun = fun;
        this.character = c;
    }

    //Clone
    public CharacterStatRelation(CharacterStatRelation cr)
    {
        this.statName = cr.statName;
        this.fun = cr.fun;
    }

    public int CalculateRelation()
    {
        float relationAdd = 0;
        switch (fun.op)
        {
            case "+":
                relationAdd += fun.value * character.GetStat(statName).GetCurrentValue();
                break;
            case "-":
                relationAdd -= fun.value * character.GetStat(statName).GetCurrentValue();
                break;
            default:
                Assert.IsTrue(false, "Invalid Operand");
                break;

        }

        if (statName == "Magic Defense")
            Debug.Log("Normal" + character.name + " " + character.GetStat(statName).GetCurrentValue());

        return (int)relationAdd;
    }

    public int CalculateRelation_Pedantically()
    {
        float relationAdd = 0;
        switch (fun.op)
        {
            case "+":
                character.GetStat(statName).CalculateCurrentValue_Pedantically();
                relationAdd += fun.value * character.GetStat(statName).GetCurrentValue();
                break;
            case "-":
                character.GetStat(statName).CalculateCurrentValue_Pedantically();
                relationAdd -= fun.value * character.GetStat(statName).GetCurrentValue();
                break;
            default:
                Assert.IsTrue(false, "Invalid Operand");
                break;

        }

        if (statName == "Magic Defense")
            Debug.Log(character.name + " " + character.GetStat(statName).GetCurrentValue());

        return (int)relationAdd;
    }

    public override string ToString()
    {
        string s = "";
        s += fun.op + " | ";
        s += fun.value.ToString() + " | ";
        return s;
    }
}