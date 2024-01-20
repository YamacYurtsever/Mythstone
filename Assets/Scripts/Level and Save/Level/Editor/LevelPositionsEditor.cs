using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LevelPositionsInspector))]
public class LevelPositionsEditor : Editor
{
    SerializedProperty m_levelPositions;

    private void OnEnable()
    {
        m_levelPositions = serializedObject.FindProperty("levelPositions");
    }

    public override void OnInspectorGUI()
    {
        LevelPositionsInspector levelPositionsInspector = (LevelPositionsInspector)target;
        EditorGUILayout.PropertyField(m_levelPositions, new GUIContent("Levels Positions"));

        if (GUILayout.Button("Level Button Positions Method 1"))
        {
            levelPositionsInspector.AdjustLevelButtonPositions();
        }

        if (GUILayout.Button("Level Button Positions Method 2"))
        {
            levelPositionsInspector.AdjustLevelButtonPositionsMethod2();
        }

        if (GUILayout.Button("Save Level Button Positions"))
        {
            levelPositionsInspector.SaveLevelPositionsToList();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
