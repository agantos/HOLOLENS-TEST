using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveActionButton : MonoBehaviour
{
    public string moveeName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        FindAnyObjectByType<CharacterMover>(FindObjectsInactive.Include).movee = GameManager.playingCharacterGameObjects[moveeName].GetComponent<NavMeshAgent>();

    }
}
