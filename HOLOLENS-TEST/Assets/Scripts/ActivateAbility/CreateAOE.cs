using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateAOE : MonoBehaviour
{
    public GameObject CubeSelectPrefab;
    public GameObject CircleSelectPrefab;
    GameObject spawned;

    public void InstantiateObject(Ability ability, Character attacker)
    {
        //Check if the ability can be cast
        if (AbilityManager.Activate_CheckCost(ability.name, attacker))
        {
            switch (ability.effects[0].areaOfEffect.shape)
            {
                case AreaShape.CUBE:
                    spawned = Instantiate(CubeSelectPrefab);
                    break;
                case AreaShape.CONE:
                    break;
                case AreaShape.SPHERE:
                    break;
                case AreaShape.LINE:
                    break;                
                case AreaShape.CIRCLE:
                    spawned = Instantiate(CircleSelectPrefab);
                    break;
                case AreaShape.SELECT:
                    break;
            }

            //Set the parameters of the radius selct script
            spawned.GetComponent<RadiusSelectScript>().attacker = attacker;
            spawned.GetComponent<RadiusSelectScript>().ability = ability;

            //Size the spawned selector
            spawned.GetComponent<RadiusSelectScript>().radius = ability.effects[0].areaOfEffect.radius;
            spawned.GetComponent<RadiusSelectScript>().SetScale();

            //Set the spawned selector's position
            spawned.transform.SetParent(this.transform.parent.transform, false);
            spawned.transform.localPosition = this.transform.localPosition;
        }
        else
            Debug.Log(attacker.name + " cannot cast ability " + ability.name);
    }

    public void InitiateSelect()
    {

    }
}
