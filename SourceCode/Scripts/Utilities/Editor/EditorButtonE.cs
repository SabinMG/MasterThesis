using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Cgl.Thesis.Utilities
{
    [CustomEditor(typeof(EditorButton))]
    public class EditorButtonE : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorButton button = (EditorButton)target;
            if (GUILayout.Button("Click"))
            {
                button.Click();
            }
        }
    }
}
