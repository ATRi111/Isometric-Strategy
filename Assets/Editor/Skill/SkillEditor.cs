using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, actionTime;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("չʾ������");
        actionTime.IntField("�̶�ʱ������");
    }
}