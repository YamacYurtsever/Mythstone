using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResetDataInspectorButton))]
public class ResetDataEditor : Editor
{
    SerializedProperty m_resetData;

    private void OnEnable()
    {
        m_resetData = serializedObject.FindProperty("resetData");
    }

    public override void OnInspectorGUI()
    {
        ResetDataInspectorButton resetDataInspector = (ResetDataInspectorButton)target;
        EditorGUILayout.PropertyField(m_resetData, new GUIContent("Reset Data"));

        if (GUILayout.Button("Reset Score Data for Level"))
        {
            resetDataInspector.ResetScoreDataforLevel();
        }

        if (GUILayout.Button("Reset Certain Star Data for Level"))
        {
            resetDataInspector.ResetCertainStarDataforLevel();
        }

        if (GUILayout.Button("Reset Star Data Completely for Level"))
        {
            resetDataInspector.ResetStarDataCompletelyforLevel();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
