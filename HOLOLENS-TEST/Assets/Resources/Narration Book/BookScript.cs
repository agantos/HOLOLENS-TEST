using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    //Set in Unity Editor
    public GameObject bookModel;
    public AudioSource pageFlipAudioSource;
    public AudioSource narrationAudioSource;

    //UI GameObjects set in editor
    public GameObject BookControls;
    public GameObject ConnectionUI;
    public GameObject StartButton;
    public GameObject ConnectButton;
    public GameObject RestartNarrationButton;

    
    //Resources to Load
    AudioClip[] narration = new AudioClip[10];
    AudioClip[] pageChangeSounds = new AudioClip[7];
    Material[] content = new Material[10];

    Renderer bookRenderer;
    System.Random random;

    //Animation 
    Animator animator;
    int startHash, endHash;

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

        //Animator properties;
        animator = GetComponent<Animator>();
        startHash = Animator.StringToHash("Start");
        endHash = Animator.StringToHash("ShouldClose");

        //Disable Some UI
        BookControls.SetActive(false);
        ConnectionUI.SetActive(false);
        ConnectButton.SetActive(false);
        RestartNarrationButton.SetActive(false);
    }

    public void BeginNarration()
    {
        //Animations
        animator.SetBool(startHash, true);
        animator.SetBool(endHash, false);

        Invoke("PlayFirstPage", 1.2f);

        //UI
        BookControls.SetActive(true);
        StartButton.SetActive(false);
        RestartNarrationButton.SetActive(false);                
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
        if(currentPlayingClip < narration.Length - 2)
        {
            if(narrationAudioSource.isPlaying)
                narrationAudioSource.Stop();

            PlayPage(currentPlayingClip + 1);
        }
        else
        {
            if (narrationAudioSource.isPlaying)
                narrationAudioSource.Stop();

            PlayLastPage();
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

    void PlayLastPage()
    {
        PlayPage(narration.Length - 1);
        Invoke("EndNarration", narration[narration.Length - 1].length + 1);
    }

    void PlayFirstPage()
    {
        PlayPage(0);
    }

    public void EndNarration()
    {
        //Animations
        animator.SetBool(endHash, true);
        animator.SetBool(startHash, false);

        //Sounds
        if (narrationAudioSource.isPlaying)
            narrationAudioSource.Stop();

        //UI
        BookControls.SetActive(false);
        ConnectionUI.SetActive(true);
        ConnectButton.SetActive(true);
        RestartNarrationButton.SetActive(true);        
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
