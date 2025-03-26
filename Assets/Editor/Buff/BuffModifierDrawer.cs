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
        AutoPropertyField("Buff����", so);
        remove.BoolField("�Ƴ�Buff", NextRectRelative());
        probability.IntSlider("ʩ����", 0, 100, NextRectRelative());
    }
}