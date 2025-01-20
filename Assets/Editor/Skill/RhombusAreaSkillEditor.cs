using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RhombusAreaSkill))]
public class RhombusAreaSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty effectRange;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        effectRange.IntField("×÷ÓÃ·¶Î§");
    }
}