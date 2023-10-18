using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject bookModel;

    AudioClip[] sounds = new AudioClip[10];
    Material[] content = new Material[10];

    Renderer bookRenderer;
    AudioSource audioSource;

    int currentPlayingClip = 0;

    // Start is called before the first frame update
    void Awake()
    {
        LoadAssets();
    }

    void Start()
    {
        bookRenderer = bookModel.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

        PlayPage(0);
    }

    void LoadAssets()
    {
        for(int i = 0; i < 10; i++)
        {
            sounds[i] = (AudioClip)Resources.Load( "Narration Book/Sounds/Page " + (i + 1) );
            content[i] = (Material)Resources.Load( "Narration Book/Pages/Page " + (i + 1) );
        }        
    }

    public void NextPage()
    {
        audioSource.Stop();
        PlayPage(currentPlayingClip + 1);
    }

    void PlayPage(int i)
    {
        currentPlayingClip = i;
        bookRenderer.material = content[i];

        Invoke("ChangeSound", 0.5f);
    }

    void ChangeSound()
    {
        audioSource.clip = sounds[currentPlayingClip];
        audioSource.Play();
    }
}
