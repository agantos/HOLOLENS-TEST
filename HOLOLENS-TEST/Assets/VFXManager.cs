using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    //Fill in Unity Editor
    public GameObject Environment;
    public AudioSource onImpactSoundEffect;
    public AudioSource duringActivationSoundEffect;

    public static VFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    ParticleSystem LoadEffect(string name)
    {
        GameObject fireball = Instantiate((GameObject)Resources.Load("VFX/" + name));
        fireball.GetComponent<ParticleSystem>().Pause();
        return fireball.GetComponent<ParticleSystem>();
    }

    Vector3 PlaceEffect(string effectName, ParticleSystem effect, Transform parent = null, Vector3 positionOffset = default(Vector3), Vector3 rotationOffset = default(Vector3))
    {
        //Keep original scale on a variable
        Vector3 originalScale = effect.gameObject.transform.localScale;

        //Set the parent of the effect
        if (parent != null)
            effect.gameObject.transform.SetParent(parent);
        else
            effect.gameObject.transform.SetParent(Environment.transform);

        //Reset the transform of the effect
        effect.gameObject.transform.localScale = originalScale;
        effect.gameObject.transform.localPosition = Vector3.zero;
       
        //Place the effect on the spot
        positionOffset = AddCustomOffset(effectName, positionOffset);
        effect.gameObject.transform.localPosition += positionOffset;

        //Rotate the effect according to the offset
        effect.gameObject.transform.Rotate(rotationOffset);
        
        return effect.gameObject.transform.localPosition;
    }

    //Create custom offset for effects if needed
    public Vector3 AddCustomOffset(string effectName, Vector3 offset)
    {
        if (effectName == "Fireball")
        {
            offset += new Vector3(0, 3f, 0);
        }
        if(effectName == "Flamethrower")
        {
            offset += new Vector3(0, 1.3f, 0.4f);
        }
        return offset;
    }

    void PlayEffect(ParticleSystem effect)
    {
        effect.Play();
    }

    public ParticleSystem ActivateVFX(string name, Transform parent, out Vector3 position, Vector3 offsetPosition = default(Vector3), Vector3 offsetRotation = default(Vector3))
    {
        ParticleSystem effect = LoadEffect(name);

        position = PlaceEffect(name, effect, parent, offsetPosition, offsetRotation);
        PlayEffect(effect);        

        return effect;
    }

    public void PlaySoundFX_onImpact(string path, Vector3 position)
    {
        path = "SoundFX/" + path;
        onImpactSoundEffect.gameObject.transform.localPosition = position;
        onImpactSoundEffect.clip = (AudioClip)Resources.Load(path);
        onImpactSoundEffect.Play();
    }

    public void PlaySoundFX_duringActivation(string path, Vector3 position)
    {    
        path = "SoundFX/" + path;
        duringActivationSoundEffect.gameObject.transform.localPosition = position;
        duringActivationSoundEffect.clip = (AudioClip)Resources.Load(path);
        duringActivationSoundEffect.Play();
    }

    public void DeactivateDuringActivationSoundFX()
    {
        duringActivationSoundEffect.Stop();
    }

    public void DeactivateVFX(ParticleSystem effect)
    {
        Destroy(effect.gameObject);
    }
}
