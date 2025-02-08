using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PawnBrain))]
public class PawnBrainEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty humanControl, personality, plans, prepared;

    protected override void MyOnInspectorGUI()
    {
        humanControl.BoolField("����ҿ���");
        if (!humanControl.boolValue)
            personality.PropertyField("����");
        if (Application.isPlaying)
        {
            plans.ListField("��ǰ�ƻ�");
            prepared.BoolField("׼�����ж�");
        }
    }
}