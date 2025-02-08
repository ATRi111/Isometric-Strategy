using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Trend))]
public class TrendDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty offerSupport, seekSupport, offense, defense, terrain;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        offerSupport.FloatField("提供协助倾向",NextRectRelative());
        seekSupport.FloatField("寻求协助倾向", NextRectRelative());
        offense.FloatField("进攻倾向", NextRectRelative());
        defense.FloatField("撤退倾向", NextRectRelative());
        terrain.FloatField("地形重视度", NextRectRelative());
    }
}