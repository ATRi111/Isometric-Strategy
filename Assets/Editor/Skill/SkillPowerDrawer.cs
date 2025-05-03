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
        type.EnumField<EDamageType>("伤害类型", NextRectRelative());
        power.IntField("威力", NextRectRelative());
        strMultiplier.FloatField("力量加成", NextRectRelative());
        dexMultiplier.FloatField("灵巧加成", NextRectRelative());
        intMultiplier.FloatField("智力加成", NextRectRelative());
        mndMultiplier.FloatField("精神加成", NextRectRelative());
    }
}