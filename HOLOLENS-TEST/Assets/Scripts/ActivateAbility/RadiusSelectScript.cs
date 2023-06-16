using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusSelectScript : MonoBehaviour
{
    public GameObject selectToken;
    public List<GameObject> tokensCreated;
    public int radius;
    //should represent what 1 feet is in the game
    public float baseRadiusX, baseRadiusY, baseRadiusZ;

    //- Adds collided Character to defenders
    //- Creates a token that indicates the collided is selected
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Spawn selection token.
            GameObject token = Object.Instantiate(selectToken);                      
            token.transform.SetParent(other.transform, false);
                        
            //Models may be rescaled but the scale will be uniform. Remove this scale from the token.
            token.transform.localScale *= 1 / other.transform.localScale.x;

            //Place selection token on top of model.
            float playerHeight = other.transform.localPosition.y + 0.5f;
            token.transform.localPosition += new Vector3(0, playerHeight + 0.3f, 0);
            tokensCreated.Add(token);

            //Add character to the selected characters
            SelectTarget(other.gameObject);
        }

    }

    //- Removes the select visual indication
    //- Removes collided from defenders
    void OnTriggerExit(Collider other)
    {
        //Only on collisions with gameobject with the correct TAG
        if(other.gameObject.tag == "Player")
        {
            //Destroy spawned selection token
            GameObject obj = other.transform.Find("IsSelected(Clone)").gameObject;
            tokensCreated.Remove(obj);
            Destroy(obj);

            //Remove character from the selected characters
            UnselectTarget(other.gameObject);
        }        
    }

    //Set the size the AOE selection
    public void SetScale()
    {
        int abilityRadiusInFeet = CastingAbilityManager.abilityToCast.effects[0].areaOfEffect.radius;
        float abilityRadiusUnityScaled = GameplayCalculatorFunctions.FeetToUnityMeasurement(abilityRadiusInFeet);
        gameObject.transform.localScale = new Vector3(abilityRadiusUnityScaled, 0.00005f, abilityRadiusUnityScaled);
    }

    void SelectTarget(GameObject target) {
        CastingAbilityManager.defendersCharacter.Add(target.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Add(target);
    }

    void UnselectTarget(GameObject target)
    {
        CastingAbilityManager.defendersCharacter.Remove(target.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.defendersGameObject.Remove(target);
    }

    public void OnActivate()
    {
        //Destroy the spawned radius effect
        Destroy(gameObject);

        //Despawn select marker
        foreach(GameObject obj in tokensCreated)
        {
            Destroy(obj);
        }
    }
}
