using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty icon, displayName, animationName, movementName, movementLatency;
    [AutoProperty]
    public SerializedProperty preConditions, buffPreConditions, actionTime, parameterOnAgent, buffOnAgent, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("չʾ������");
        if (GUILayout.Button("�Զ����"))
        {
            displayName.stringValue = GenerateDisplayName();
        }
        animationName.TextField("������Ч��");
        movementName.TextField("���ﶯ����");
        if (!string.IsNullOrEmpty(movementName.stringValue))
            movementLatency.FloatField("���ﶯ���ӳ�");
        icon.PropertyField("ͼ��");
        preConditions.ListField("����ǰ������");
        buffPreConditions.ListField("Buffǰ������");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        actionTime.IntField("�̶�ʱ������");
        parameterOnAgent.ListField("�����޸�");
        buffOnAgent.ListField("������ʩ�ӵ�Buff");
        extraDescription.TextArea("��������");
        EditorGUILayout.LabelField("��������");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as Skill).Description);
        EditorGUI.EndDisabledGroup();
    }

    public string GenerateDisplayName()
    {
        string fileName = target.name;
        string[] ss = fileName.Split('_');
        return ss[^1];
    }
}