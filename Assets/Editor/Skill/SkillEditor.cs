using EditorExtend;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty icon, displayName, animationName, pawnAnimationState, movementLatency, weaponAnimation;
    [AutoProperty]
    public SerializedProperty preConditions, buffPreConditions, weatherPreConditions, actionTime, HPCost, parameterOnAgent, buffOnAgent, extraDescription;

    protected List<string> animationNames;

    protected override void OnEnable()
    {
        base.OnEnable();
        animationNames = EditorExtendUtility.FindAssetsNames($"������Ч t:GameObject");
    }

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("չʾ������");
        if (GUILayout.Button("�Զ����"))
        {
            displayName.stringValue = GenerateDisplayName();
        }
        icon.PropertyField("ͼ��");
        animationName.TextFieldWithOptionButton("������Ч��", animationNames);
        pawnAnimationState.EnumField<EPawnAnimationState>("���ﶯ��");
        movementLatency.FloatField("���ﶯ���ӳ�");
        weaponAnimation.BoolField("������������");
        preConditions.ListField("����ǰ������");
        buffPreConditions.ListField("Buffǰ������");
        weatherPreConditions.ListField("����ǰ������");
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