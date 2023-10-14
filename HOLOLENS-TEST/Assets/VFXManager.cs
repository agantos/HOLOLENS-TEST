using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject Environment;
    public static VFXManager Instance { get; private set; }

    private Dictionary<string, GameObject> loadedVFX;

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

    void PlaceEffect(string effectName, ParticleSystem effect, Transform parent = null, Vector3 offset = default(Vector3))
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
        offset = AddOffset(effectName, offset);
        effect.gameObject.transform.localPosition += offset;                  
    }

    void PlaceEffectAtBottomOfCharacter(string effectName, ParticleSystem effect, Transform character, Vector3 offset)
    {
        
    }

    //Create custom offset for effects if needed
    public Vector3 AddOffset(string effectName, Vector3 offset)
    {
        if (effectName == "Fireball")
        {
            offset += new Vector3(0, 3f, 0);
        }
        return offset;
    }

    void PlayEffect(ParticleSystem effect)
    {
        effect.Play();
    }

    public ParticleSystem ActivateVFX(string name, Transform parent, Vector3 offset = default(Vector3))
    {
        ParticleSystem effect = LoadEffect(name);

        PlaceEffect(name, effect, parent, offset);
        PlayEffect(effect);       

        return effect;
    }

    public void DeactivateVFX(ParticleSystem effect)
    {
        Destroy(effect.gameObject);
    }
}
