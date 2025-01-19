using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ParameterPreCondition))]
public class ParameterPreConditionDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty conditionType, parameterIndex, value;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PawnParameterModifierDrawer.ParameterIndexField(parameterIndex, NextRectRelative());
        conditionType.EnumField<EParameterConditionType>("����", NextRectRelative());
        value.IntField("ֵ", NextRectRelative());
    }
}