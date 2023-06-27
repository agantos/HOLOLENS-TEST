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

        //component.spawnRadius = (GameObject)EditorGUILayout.ObjectField("SpawnRadius", component.spawnRadius, typeof(GameObject), true);
        //component.activateButtonPrefab = (GameObject)EditorGUILayout.ObjectField("Activate Button Prefab", component.activateButtonPrefab, typeof(GameObject), true);
        //component.cancelAbilityButtonPrefab = (GameObject)EditorGUILayout.ObjectField("Cancel Ability Button Prefab", component.cancelAbilityButtonPrefab, typeof(GameObject), true);
        //component.abilityName = EditorGUILayout.TextField("Ability Name", component.abilityName);
        //component.attackerName = EditorGUILayout.TextField("Attacker Name", component.attackerName);
    }
} 