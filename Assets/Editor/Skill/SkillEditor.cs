using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, preConditions, buffPreConditions, actionTime, parameterOnAgent, buffOnAgent, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        preConditions.ListField("����ǰ������");
        buffPreConditions.ListField("Buffǰ������");
        displayName.TextField("չʾ������");
        actionTime.IntField("�̶�ʱ������");
        parameterOnAgent.ListField("�����޸�");
        buffOnAgent.ListField("������ʩ�ӵ�Buff");
        extraDescription.TextArea("��������");
        EditorGUILayout.LabelField("��������");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as Skill).Description);
        EditorGUI.EndDisabledGroup();
    }
}