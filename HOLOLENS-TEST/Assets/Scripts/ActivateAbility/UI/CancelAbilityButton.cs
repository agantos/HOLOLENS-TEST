using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Singleton Button that cancels the activation of current selected ability
public class CancelAbilityButton : Button
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying)
                    return;
#endif

        onClick.AddListener(delegate {
            CastingAbilityManager.DeactivateAbility();
        });

        Deactivate();
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }

    public void Activate()
    {
        this.gameObject.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CastingAbilityManager.DeactivateAbility();
    }
}
