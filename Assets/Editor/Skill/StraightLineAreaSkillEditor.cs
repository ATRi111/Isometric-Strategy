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
        length.IntField("作用范围长度");
        width.IntField("作用范围宽度");
        if (width.intValue % 2 == 0)
            width.intValue++;
    }
}