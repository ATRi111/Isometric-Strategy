using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AIManager))]
public class AIManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty trend, valueMinMultiplier, valueMaxMultiplier;

    protected override void MyOnInspectorGUI()
    {
        trend.PropertyField("AI总体倾向");
        EditorExtendGUI.MinMaxSlider(valueMinMultiplier, valueMaxMultiplier, "AI评分波动范围", 0f, 2f);
        EditorGUILayout.BeginHorizontal();
        valueMinMultiplier.FloatField("");
        valueMaxMultiplier.FloatField("");
        EditorGUILayout.EndHorizontal();
    }
}