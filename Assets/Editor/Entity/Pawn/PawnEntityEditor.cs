using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : EntityEditor
{
    [AutoProperty]
    public SerializedProperty icon, faction, taskTarget, pClass, race, actionTime, speedUpRate, time, hidden;

    public bool foldout;

    protected override void MyOnInspectorGUI()
    {   
        base.MyOnInspectorGUI();
        icon.PropertyField("ͷ��");
        faction.EnumField<EFaction>("��Ӫ");
        taskTarget.BoolField("����Ŀ��");
        pClass.PropertyField("ְҵ");
        race.PropertyField("����");
        actionTime.PropertyField("�����ж�ʱ��");
        speedUpRate.PropertyField("������");
        time.IntField("�ۻ��ȴ�ʱ��");
        hidden.BoolField("���ɼ���ɫ");
        foldout = AutoDictionaryDrawerHelper.OnInspectorGUI(foldout, "��ɫ����", (target as PawnEntity).parameterDict.dict);
    }
}