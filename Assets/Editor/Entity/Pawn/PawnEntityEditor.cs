using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pClass, pRace, actionTime, time;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("��Ӫ");
        pClass.PropertyField("ְҵ");
        pRace.PropertyField("����");
        actionTime.PropertyField("�����ж�ʱ��");
        time.IntField("�ۻ��ȴ�ʱ��");
    }
}