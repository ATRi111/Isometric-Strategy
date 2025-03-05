using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ParabolaPreciseShootSkill))]
public class ParabolaPreciseShootSkillEditor : ParabolaSkillEditor
{
    [AutoProperty]
    public SerializedProperty damageAmplifier, so;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        so.PropertyField("����״̬");
        damageAmplifier.FloatField("�˺�����");
    }
}