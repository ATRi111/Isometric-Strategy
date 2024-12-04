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
        angles.ListField("��ѡ�����");
        maxSpeed.FloatField("�������ٶ�");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.FloatField("�����о���", (target as ParabolaSkill).MaxProjectDistance());
        EditorGUI.EndDisabledGroup();
    }
}