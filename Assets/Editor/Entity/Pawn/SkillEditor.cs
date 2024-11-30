using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, actionTime, accuracy, powers;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("չʾ������");
        actionTime.IntField("�ж�ʱ��");
        accuracy.IntSlider("������", 0, Skill.MaxAccuracy);
        powers.ListField("����");
    }
}