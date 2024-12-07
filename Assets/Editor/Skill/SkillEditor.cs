using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, preConditions, actionTime, parameterModifiers;

    protected override void MyOnInspectorGUI()
    {
        preConditions.ListField("前置条件");
        displayName.TextField("展示技能名");
        actionTime.IntField("固定时间消耗");
        parameterModifiers.ListField("参数修改");
    }
}