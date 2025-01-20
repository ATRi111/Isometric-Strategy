using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(StraightLineAreaSkill))]
public class StraightLineAreaSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty length;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        length.IntField("×÷ÓÃ¾àÀë");
    }
}