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

    //Variables for the activation of the ability
    public Character attacker;
    List<Character> defenders = new List<Character>();
    public Ability ability;

    //- Adds collided Character to defenders
    //- Creates a token that indicates the collided is selected
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Spawn selection token on top of player model.
            GameObject token = Object.Instantiate(selectToken);            
            token.transform.SetParent(other.transform.parent.transform, false);
            float playerHeight = other.transform.localPosition.y + other.transform.localScale.y / 2;
            token.transform.localPosition += new Vector3(0, playerHeight + 0.1f, 0);
            tokensCreated.Add(token);

            //Add character to the selected characters
            SelectTarget(other.gameObject.transform.parent.gameObject);
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
            GameObject obj = other.gameObject.transform.parent.transform.Find("IsSelected(Clone)").gameObject;
            tokensCreated.Remove(obj);
            Destroy(obj);

            //Remove character from the selected characters
            UnselectTarget(other.gameObject.transform.parent.gameObject);
        }        
    }

    //Set the size the AOE selection
    public void SetScale()
    {
        gameObject.transform.localScale = new Vector3(radius * baseRadiusX, radius * baseRadiusY, radius * baseRadiusZ);
    }

    void SelectTarget(GameObject target) {
        defenders.Add(target.GetComponent<CharacterScript>().GetCharacter());
    }

    void UnselectTarget(GameObject target)
    {
        defenders.Remove(target.GetComponent<CharacterScript>().GetCharacter());
    }

    public void Activate()
    {
        //Activate the ability        
        attacker.ActivateOwnedAbility(ability.name, defenders, attacker);       
        
        //Destroy the spawned radius effect
        Destroy(gameObject);

        //Despawn select marker
        foreach(GameObject obj in tokensCreated)
        {
            Destroy(obj);
        }
    }
}
