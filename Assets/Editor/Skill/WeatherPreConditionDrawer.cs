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
        AutoPropertyField("天气", weather);
        requireExist.BoolField("要求处于", NextRectRelative());
    }
}