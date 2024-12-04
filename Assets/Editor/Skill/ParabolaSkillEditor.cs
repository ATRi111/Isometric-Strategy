using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ParabolaSkill), true)]
public class ParabolaSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty angles, maxSpeed;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        angles.ListField("可选发射角");
        maxSpeed.FloatField("最大出射速度");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("最大飞行距离", (target as ParabolaSkill).MaxProjectDistance());
        EditorGUI.EndDisabledGroup();
    }
}