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
    public CharacterStat stat;
    public StatFunctor fun;

    public CharacterStatRelation(CharacterStat stat, StatFunctor fun)
    {
        this.stat = stat;
        this.fun = fun;
    }

    public int CalculateRelation()
    {
        float relationAdd = 0;
        switch (fun.op)
        {
            case "+":
                relationAdd += fun.value * stat.GetCurrentValue();
                break;
            case "-":
                relationAdd -= fun.value * stat.GetCurrentValue();
                break;
            default:
                Assert.IsTrue(false, "Invalid Operand");
                break;

        }
        return (int)relationAdd;
    }

    public int CalculateRelation_Pedantically()
    {
        float relationAdd = 0;
        switch (fun.op)
        {
            case "+":
                stat.CalculateCurrentValue_Pedantically();
                relationAdd += fun.value * stat.GetCurrentValue();
                break;
            case "-":
                stat.CalculateCurrentValue_Pedantically();
                relationAdd -= fun.value * stat.GetCurrentValue();
                break;
            default:
                Assert.IsTrue(false, "Invalid Operand");
                break;

        }
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