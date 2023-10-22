using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject bookModel;
    public AudioSource pageFlipAudioSource;
    public AudioSource narrationAudioSource;

    System.Random random;

    AudioClip[] narration = new AudioClip[10];
    AudioClip[] pageChangeSounds = new AudioClip[7];
    Material[] content = new Material[10];

    Renderer bookRenderer;


    int currentPlayingClip = 0;

    // Start is called before the first frame update
    void Awake()
    {
        LoadAssets();
    }

    void Start()
    {
        bookRenderer = bookModel.GetComponent<Renderer>();
        random = new System.Random();

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
            narrationAudioSource.Stop();
            PlayPage(currentPlayingClip + 1);
        }        
    }

    public void PreviousPage()
    {
        if(currentPlayingClip > 0)
        {
            narrationAudioSource.Stop();
            PlayPage(currentPlayingClip - 1);
        }        
    }

    void PlayPage(int i)
    {
        currentPlayingClip = i;
        bookRenderer.material = content[i];

        PlayPageSound();
        Invoke("ChangeSound", 1.5f);
    }

    void PlayPageSound()
    {
        pageFlipAudioSource.clip = pageChangeSounds[random.Next(0, pageChangeSounds.Length - 1)];
        pageFlipAudioSource.Play();
    }

    void ChangeSound()
    {
        narrationAudioSource.clip = narration[currentPlayingClip];
        narrationAudioSource.Play();
    }
}
