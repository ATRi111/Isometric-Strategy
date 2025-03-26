using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffModifier))]
public class BuffModifierDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty so, remove, probability;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AutoPropertyField("Buff类型", so);
        remove.BoolField("移除Buff", NextRectRelative());
        probability.IntSlider("施加率", 0, 100, NextRectRelative());
    }
}