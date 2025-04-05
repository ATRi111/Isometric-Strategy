using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleField))]
public class BattleFieldEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty nextResetTime, weather;

    private BattleField battleField;
    private EWeather temp;

    protected override void OnEnable()
    {
        base.OnEnable();
        battleField = target as BattleField;
        temp = battleField.Weather;
    }

    protected override void MyOnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        weather.EnumField<EWeather>("当前天气");
        weather.IntField("当前天气重置时间");
        EditorGUI.EndDisabledGroup();
        if (Application.isPlaying)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("当前天气剩余时间", battleField.WeatherRemainingTime);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            temp = (EWeather)EditorGUILayout.EnumPopup("", temp);
            if (GUILayout.Button("刷新天气"))
            {
                battleField.Weather = temp;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}