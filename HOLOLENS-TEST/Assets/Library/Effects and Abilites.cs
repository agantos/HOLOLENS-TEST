using System.Collections;
using System.Collections.Generic;
interface HitEffect
{

}

interface AutomaticEffect
{

}

interface SavingEffect
{

}

enum EffectTargetType { SELF, ALLY, ENEMY, CREATURE, CREATURE_NOT_SELF, AREA}
enum EffectTargetNumber {NUMBERED, IN_RADIUS}
enum EffectShape { CUBE, CONE, SPHERE, LINE, NONE}
enum EffectType {STAT, TERRAIN, MOVE, TURN_ECONOMY}
interface Effect
{

    public (EffectShape shape, int size) effectShape { get; set; }
    public EffectTargetType targetType { get; set; }
    int range { get; set; }
    //Returns the targets of the effect
    List<int> Target();
    //Returns a bool whether the effect works on a target
    bool canApply(int Target);
    //Performs an effect
    void Apply(int Target);

}

public class Ability
{
    private string name;
    private string description;
    private int duration;
    List<Effect> effects;
}
