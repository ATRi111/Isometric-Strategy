using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pClass, race, actionTime, speedUpRate, time;

    public bool foldout;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("��Ӫ");
        pClass.PropertyField("ְҵ");
        race.PropertyField("����");
        actionTime.PropertyField("�����ж�ʱ��");
        speedUpRate.PropertyField("������");
        time.IntField("�ۻ��ȴ�ʱ��");
        foldout = AutoDictionaryDrawerHelper.OnInspectorGUI(foldout, "��ɫ����", (target as PawnEntity).parameterDict.dict);
    }
}