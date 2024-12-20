using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SkillPreCondition))]
public class SkillPreConditionDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty conditionType, parameterIndex, value;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        PawnParameterModifierDrawer.ParameterIndexField(parameterIndex, NextRectRelative());
        conditionType.EnumField<EConditionType>("����", NextRectRelative());
        value.IntField("ֵ", NextRectRelative());
    }
}