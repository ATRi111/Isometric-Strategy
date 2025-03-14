using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(MarchSkill))]
public class MarchSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty deltaTimePerLevel;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        deltaTimePerLevel.IntField("时间改变量");
    }
}