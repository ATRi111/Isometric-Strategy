using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeatherModifier))]
public class WeatherModifierDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty weather, probability;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        weather.EnumField<EWeather>("Ŀ������", NextRectRelative());
        probability.IntSlider("����", 0, 100, NextRectRelative());
    }
}