using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pawnClass, pawnRace, actionTime, time;

    protected override void MyOnInspectorGUI()
    {
        faction.EnumField<EFaction>("��Ӫ");
        pawnClass.PropertyField("ְҵ");
        pawnRace.PropertyField("����");
        actionTime.IntField("�����ж�ʱ��");
        time.IntField("�ۻ��ȴ�ʱ��");
    }
}