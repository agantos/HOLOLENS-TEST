using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[CustomEditor(typeof(BeginAbilityActivationButton))]
public class UISegmentedControlButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        BeginAbilityActivationButton component = (BeginAbilityActivationButton)target;

        base.OnInspectorGUI();

        //component.SpawnRadius = (GameObject)EditorGUILayout.ObjectField("SpawnRadius", component.SpawnRadius, typeof(GameObject), true);
        //component.ActivateButtonPrefab = (GameObject)EditorGUILayout.ObjectField("Activate Button Prefab", component.ActivateButtonPrefab, typeof(GameObject), true);
        //component.abilityName = EditorGUILayout.TextField("Ability Name", component.abilityName);
        //component.attackerName = EditorGUILayout.TextField("Attacker Name", component.attackerName);
    }
} 