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
        weather.EnumField<EWeather>("��ǰ����");
        weather.IntField("��ǰ��������ʱ��");
        EditorGUI.EndDisabledGroup();
        if (Application.isPlaying)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("��ǰ����ʣ��ʱ��", battleField.WeatherRemainingTime);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginHorizontal();
            temp = (EWeather)EditorGUILayout.EnumPopup("", temp);
            if (GUILayout.Button("ˢ������"))
            {
                battleField.Weather = temp;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}