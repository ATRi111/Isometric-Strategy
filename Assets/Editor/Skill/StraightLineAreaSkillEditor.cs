using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(StraightLineAreaSkill))]
public class StraightLineAreaSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty length, width;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        length.IntField("���÷�Χ����");
        width.IntField("���÷�Χ���");
        if (width.intValue % 2 == 0)
            width.intValue++;
    }
}