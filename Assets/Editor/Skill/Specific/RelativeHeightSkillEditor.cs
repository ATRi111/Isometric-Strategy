using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RelativeHeightSkill))]
public class RelativeHeightSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty higher, maxAmplifier, damageAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        higher.BoolField("��Ŀ���ʱ����˺�");
        damageAmplifier.FloatField("ÿ���˺�����");
        maxAmplifier.FloatField("�˺���������");
    }
}