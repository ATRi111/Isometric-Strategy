using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeatherPreCondition))]
public class WeatherPreConditionDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty weather, requireExist;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AutoPropertyField("����", weather);
        requireExist.BoolField("Ҫ����", NextRectRelative());
    }
}