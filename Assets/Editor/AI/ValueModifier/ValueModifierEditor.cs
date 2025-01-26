using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ValueModifier), true)]
public class ValueModifierEditor : AutoEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();
        appendInspector = true;
    }

    protected override void MyOnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as ValueModifier).Description);
        EditorGUI.EndDisabledGroup();
    }
}