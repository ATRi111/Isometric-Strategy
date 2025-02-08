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
        offerSupport.FloatField("�ṩЭ������",NextRectRelative());
        seekSupport.FloatField("Ѱ��Э������", NextRectRelative());
        offense.FloatField("��������", NextRectRelative());
        defense.FloatField("��������", NextRectRelative());
        terrain.FloatField("�������Ӷ�", NextRectRelative());
    }
}