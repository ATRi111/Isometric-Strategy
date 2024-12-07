using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(JumpSkill))]
public class JumpSkillEditor : MoveSkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance, jumpAngle;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField("ˮƽ��Ծ����");
        jumpAngle.FloatField("�����Ƕ�");
    }
}