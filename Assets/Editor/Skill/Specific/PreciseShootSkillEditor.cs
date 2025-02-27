using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PreciseShootSkill))]
public class PreciseShootSkillEditor : ProjectileSkillEditor
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