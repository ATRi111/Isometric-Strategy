using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, targetToKill, pClass, race, actionTime, speedUpRate, time, hidden;

    public bool foldout;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("��Ӫ");
        if ((EFaction)faction.enumValueIndex == EFaction.Enemy)
            targetToKill.BoolField("����Ŀ��");
        else
            targetToKill.boolValue = false;
        pClass.PropertyField("ְҵ");
        race.PropertyField("����");
        actionTime.PropertyField("�����ж�ʱ��");
        speedUpRate.PropertyField("������");
        time.IntField("�ۻ��ȴ�ʱ��");
        hidden.BoolField("���ɼ���ɫ");
        foldout = AutoDictionaryDrawerHelper.OnInspectorGUI(foldout, "��ɫ����", (target as PawnEntity).parameterDict.dict);
    }
}