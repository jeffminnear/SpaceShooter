using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlashText))]
public class FlashTextEditor : UnityEditor.UI.TextEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        FlashText text = (FlashText)target;
        text.flashable = EditorGUILayout.Toggle("Flashable", text.flashable);
    }
}