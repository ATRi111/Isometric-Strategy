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
        AutoPropertyField("Buff����", so);
        probability.IntSlider("ʩ����", 0, 100, NextRectRelative());
    }
}