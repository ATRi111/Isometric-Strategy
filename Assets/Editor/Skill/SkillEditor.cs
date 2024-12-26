using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, preConditions, actionTime, parameterOnAgent, buffOnAgent, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        preConditions.ListField("前置条件");
        displayName.TextField("展示技能名");
        actionTime.IntField("固定时间消耗");
        parameterOnAgent.ListField("参数修改");
        buffOnAgent.ListField("对自身施加的Buff");
        extraDescription.TextArea("额外描述");
        EditorGUILayout.LabelField("技能描述");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as Skill).Description);
        EditorGUI.EndDisabledGroup();
    }
}