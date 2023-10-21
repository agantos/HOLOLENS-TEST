using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject bookModel;

    AudioClip[] narration = new AudioClip[10];
    AudioClip[] pageChangeSounds = new AudioClip[7];
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
            narration[i] = (AudioClip)Resources.Load( "Narration Book/Sounds/Narration/Page " + (i + 1) );
            content[i] = (Material)Resources.Load( "Narration Book/Pages/Page " + (i + 1) );

            if (i < 7)
                pageChangeSounds[i] = (AudioClip)Resources.Load("Narration Book/Sounds/SFX/open page " + (i + 1));
        }        
    }

    public void NextPage()
    {
        if(currentPlayingClip < 9)
        {
            audioSource.Stop();
            PlayPage(currentPlayingClip + 1);
        }        
    }

    public void PreviousPage()
    {
        if(currentPlayingClip > 0)
        {
            audioSource.Stop();
            PlayPage(currentPlayingClip - 1);
        }        
    }

    void PlayPage(int i)
    {
        currentPlayingClip = i;
        bookRenderer.material = content[i];

        Invoke("ChangeSound", 1f);
    }

    void ChangeSound()
    {
        audioSource.clip = narration[currentPlayingClip];
        audioSource.Play();
    }
}
