using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PawnBrain))]
public class PawnBrainEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty humanControl, plans, prepared, learnedSkills;

    protected override void MyOnInspectorGUI()
    {
        humanControl.BoolField("����ҿ���");
        learnedSkills.PropertyField("����");
        if(Application.isPlaying)
        {
            plans.ListField("��ǰ�ƻ�");
            prepared.BoolField("׼�����ж�");
        }
    }
}