using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, actionTime;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("展示技能名");
        actionTime.IntField("固定时间消耗");
    }
}