using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSelectScript : MonoBehaviour
{    
    public GameObject selectToken;
    public GameObject positionAtEnd;
    public Transform arrowBody;

    public static LineSelectScript Instance { get; private set; }

    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager objects
        }
    }

    List<GameObject> tokensCreated = new List<GameObject>();

    public void LookAtPosition(Vector3 position) {
        position.y = transform.position.y;
        transform.LookAt(position);

        //Because of Line model Shanenigans
        transform.Rotate(0, 90, 0);
    }

    public void SetScale(float range, float radius)
    {
        float unityScaleRange = GameplayCalculatorFunctions.FeetToUnityMeasurement(range);
        float unityScaleRadius = GameplayCalculatorFunctions.FeetToUnityMeasurement(radius);

        //Size according to ability
        arrowBody.localScale = new Vector3(arrowBody.localScale.x * (unityScaleRange - 1), arrowBody.localScale.y, arrowBody.localScale.z);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * unityScaleRadius);

        //Position arrow transform to the end of its parent in order to rotate normally
        positionAtEnd.transform.localPosition = new Vector3(-(unityScaleRange), 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Don't account the caster of the ability
            if (other.gameObject.GetComponent<CharacterScript>().charName != CastingAbilityManager.Instance.attacker.name)
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

    }

    //- Removes the select visual indication
    //- Removes collided from defenders
    void OnTriggerExit(Collider other)
    {
        //Only on collisions with gameobject with the correct TAG
        if (other.gameObject.tag == "Player")
        {
            //Don't account the caster of the ability
            if (other.gameObject.GetComponent<CharacterScript>().charName != CastingAbilityManager.Instance.attacker.name)
            {
                //Destroy spawned selection token
                GameObject obj = other.transform.Find("IsSelected(Clone)").gameObject;
                tokensCreated.Remove(obj);
                Destroy(obj);

                //Remove character from the selected characters
                UnselectTarget(other.gameObject);
            }            
        }
    }

    void SelectTarget(GameObject target)
    {
        CastingAbilityManager.GetInstance().defenderCharacters.Add(target.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.GetInstance().defendersGameObject.Add(target);
    }

    void UnselectTarget(GameObject target)
    {
        CastingAbilityManager.GetInstance().defenderCharacters.Remove(target.GetComponent<CharacterScript>().GetCharacter());
        CastingAbilityManager.GetInstance().defendersGameObject.Remove(target);
    }

    public void OnAbilityActivate()
    {
        //Destroy the Line
        Instance = null;
        Destroy(gameObject);

        //Despawn select marker
        foreach (GameObject obj in tokensCreated)
        {
            Destroy(obj);
        }
    }

    public void OnAbilityDeActivate()
    {
        //Destroy the spawned radius effect
        Instance = null;
        Destroy(gameObject);

        //Despawn select marker
        foreach (GameObject obj in tokensCreated)
        {
            Destroy(obj);
        }
    }
}
