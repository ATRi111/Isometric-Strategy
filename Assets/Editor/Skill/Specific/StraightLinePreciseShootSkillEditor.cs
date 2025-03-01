using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(StraightLinePreciseShootSkill))]
public class StraightLinePreciseShootSkillEditor : ProjectileSkillEditor
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