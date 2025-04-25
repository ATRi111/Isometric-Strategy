using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty icon, displayName, animationPrefab, pawnAnimationState, movementLatency, weaponAnimation;
    [AutoProperty]
    public SerializedProperty preConditions, buffPreConditions, specialPreconditions, actionTime, HPCost, parameterOnAgent, buffOnAgent, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("չʾ������");
        if (GUILayout.Button("�Զ����"))
        {
            displayName.stringValue = GenerateDisplayName();
        }
        icon.PropertyField("ͼ��");
        animationPrefab.PropertyField("������Ч");
        pawnAnimationState.EnumField<EPawnAnimationState>("���ﶯ��");
        movementLatency.FloatField("���ﶯ���ӳ�");
        weaponAnimation.BoolField("������������");
        preConditions.ListField("����ǰ������");
        buffPreConditions.ListField("Buffǰ������");
        specialPreconditions.ListField("����ǰ������");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        actionTime.IntField("����ʱ������");
        HPCost.IntField("Ѫ������");
        parameterOnAgent.ListField("�����޸�");
        buffOnAgent.ListField("������ʩ��/�Ƴ�Buff");
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