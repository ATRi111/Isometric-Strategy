using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffModifier))]
public class BuffModifierDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty so, probability;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AutoPropertyField("Buff类型", so);
        probability.IntSlider("施加率", 0, 100, NextRectRelative());
    }
}