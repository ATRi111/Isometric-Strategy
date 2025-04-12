using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(SneakAttackSkill))]
public class SneakAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty damageAmplifier, ignorePierceResistance;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        damageAmplifier.FloatField("�����˺�����");
        ignorePierceResistance.IntField("���ӻ���");
    }
}