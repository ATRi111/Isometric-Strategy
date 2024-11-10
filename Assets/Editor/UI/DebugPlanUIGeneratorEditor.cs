using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DebugPlanUIGenerator))]
public class DebugPlanUIGeneratorEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty prefab, brain, paintIndex, skills;
    private DebugPlanUIGenerator generator;
    public string[] options;
    private int prev;

    protected override void OnEnable()
    {
        base.OnEnable();
        prev = -1;
        generator = target as DebugPlanUIGenerator;
    }

    protected override void MyOnInspectorGUI()
    {
        prefab.PropertyField("预制体");
        brain.PropertyField("行动者");
        options = new string[generator.skills.Count + 1];
        options[0] = "最佳计划";
        for (int i = 0; i < generator.skills.Count; i++)
        {
            options[i + 1] = generator.skills[i].name;
        }
        paintIndex.intValue = EditorGUILayout.Popup(new GUIContent("绘制选项"), paintIndex.intValue, options);
        if (Application.isPlaying && paintIndex.intValue != prev)
        {
            serializedObject.ApplyModifiedProperties();
            generator.Paint();
            prev = paintIndex.intValue;
        }
    }
}