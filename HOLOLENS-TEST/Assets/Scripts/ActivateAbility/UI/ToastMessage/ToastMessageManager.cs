using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastMessageManager : MonoBehaviour
{
    public GameObject ToastMessagePrefab;

    static public ToastMessageManager Instance {get; private set;}

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

    }

    public void CreateToast(string s)
    {
        GameObject toast = Instantiate(ToastMessagePrefab, transform);
        toast.GetComponent<ToastMessage>().SetText(s);
        toast.GetComponent<ToastMessage>().DestroyInSeconds(4);
    }



}
