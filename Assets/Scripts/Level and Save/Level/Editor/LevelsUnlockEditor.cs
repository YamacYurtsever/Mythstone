using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelsUnlockInspector))]
public class LevelsUnlockEditor : Editor
{
    SerializedProperty m_levelsUnlock;

    private void OnEnable()
    {
        m_levelsUnlock = serializedObject.FindProperty("levelsUnlock");
    }

    public override void OnInspectorGUI()
    {
        LevelsUnlockInspector levelsUnlockInspector = (LevelsUnlockInspector)target;
        EditorGUILayout.PropertyField(m_levelsUnlock, new GUIContent("Levels Unlock"));

        if(GUILayout.Button("Levels Design 1"))
        {
            levelsUnlockInspector.LevelDesign1();
        }

        if (GUILayout.Button("Levels Design 2"))
        {
            levelsUnlockInspector.LevelDesign2();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
