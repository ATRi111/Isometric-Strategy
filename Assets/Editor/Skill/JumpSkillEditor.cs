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
        castingDistance.IntField("水平跳跃距离");
        jumpAngle.FloatField("起跳角度");
    }
}