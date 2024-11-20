using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PawnPropertyModifier))]
public class PawnPropertyModifierDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty modifiers;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Initialize(position, property);
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel++;
        MyOnGUI(position, property, label);
        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        modifiers.ListField("¥ Ãı");
    }
}