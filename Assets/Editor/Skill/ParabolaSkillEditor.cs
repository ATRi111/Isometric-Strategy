using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ParabolaSkill), true)]
public class ParabolaSkillEditor : ProjectileSkillEditor
{
    [AutoProperty]
    public SerializedProperty angles, maxSpeed;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        angles.ListField("可选发射角");
        maxSpeed.FloatField("最大出射速度");
        EditorGUI.BeginDisabledGroup(true);
        float f = (target as ParabolaSkill).MaxProjectDistance(0);
        float h = (target as ParabolaSkill).MaxProjectDistance(ParabolaSkill.DefaultHeight);
        EditorGUILayout.FloatField("最大飞行距离(平地)", f);
        EditorGUILayout.FloatField("最大飞行距离(高处)", h);
    }
}