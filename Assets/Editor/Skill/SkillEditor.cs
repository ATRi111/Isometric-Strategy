using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, preConditions, actionTime, parameterModifiers, buffOnAgent;

    protected override void MyOnInspectorGUI()
    {
        preConditions.ListField("ǰ������");
        displayName.TextField("չʾ������");
        actionTime.IntField("�̶�ʱ������");
        parameterModifiers.ListField("�����޸�");
        buffOnAgent.ListField("������ʩ�ӵ�Buff");
    }
}