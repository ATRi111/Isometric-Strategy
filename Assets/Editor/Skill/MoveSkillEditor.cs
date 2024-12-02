using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(MoveSkill), true)]
public class MoveSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty actionTimePerUnit, ZOCActionTime, speedMultiplier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        actionTimePerUnit.IntField("ÿ��ʱ������");
        ZOCActionTime.IntField("ZOCʱ������");
        speedMultiplier.FloatField("�����ٶ�");
    }
}