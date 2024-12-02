using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PawnPropertyModifier))]
public class PawnPropertyModifierDrawer : AutoPropertyDrawer
{
    public override bool NoLabel => true;
    [AutoProperty]
    public SerializedProperty modifiers;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        modifiers.ListField("¥ Ãı");
    }
}