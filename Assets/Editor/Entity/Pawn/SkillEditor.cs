using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty actionTime, accuracy, powers;

    protected override void MyOnInspectorGUI()
    {
        actionTime.IntField("�ж�ʱ��");
        accuracy.IntSlider("������", 0, Skill.MaxAccuracy);
        powers.ListField("����");
    }
}