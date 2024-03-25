using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// THIS CODE BELONGS TO THE USER: Unity Adventure in youtube. Extracted from his video: ¿Cómo usar ChatGPT en Unity? - OpenAI API
/// </summary>

[CustomEditor(typeof(ChatGpt_Communnication))]
public class Editor_Interface : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChatGpt_Communnication script = (ChatGpt_Communnication)target;

        GUILayout.Space(15);

        if (GUILayout.Button("Ask"))
        {
            script.sendRequest();
        }

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear")) {
            script.clear();
        }

        GUILayout.EndHorizontal();
    }
}
