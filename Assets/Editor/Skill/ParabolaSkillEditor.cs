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
        angles.ListField("��ѡ�����");
        maxSpeed.FloatField("�������ٶ�");
        EditorGUI.BeginDisabledGroup(true);
        float f = (target as ParabolaSkill).MaxProjectDistance(0);
        float h = (target as ParabolaSkill).MaxProjectDistance(ParabolaSkill.DefaultHeight);
        EditorGUILayout.FloatField("�����о���(ƽ��)", f);
        EditorGUILayout.FloatField("�����о���(�ߴ�)", h);
    }
}