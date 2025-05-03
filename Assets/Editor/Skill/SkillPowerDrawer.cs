using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SkillPower))]
public class SkillPowerDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty type, power, strMultiplier, dexMultiplier, intMultiplier, mndMultiplier;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        type.EnumField<EDamageType>("�˺�����", NextRectRelative());
        power.IntField("����", NextRectRelative());
        strMultiplier.FloatField("�����ӳ�", NextRectRelative());
        dexMultiplier.FloatField("���ɼӳ�", NextRectRelative());
        intMultiplier.FloatField("�����ӳ�", NextRectRelative());
        mndMultiplier.FloatField("����ӳ�", NextRectRelative());
    }
}