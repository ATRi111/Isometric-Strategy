using EditorExtend;
using EditorExtend.GridEditor;
using UnityEditor;

[CustomEditor(typeof(Pawn))]
public class PawnEditor : GridObjectEditor
{
    [AutoProperty]
    public SerializedProperty faction, climbAbility, dropAbility, moveAbility;

    protected override void MyOnInspectorGUI()
    {
        faction.EnumField<EFaction>("��Ӫ");
        climbAbility.IntField("��������߶�");
        dropAbility.IntField("�������߶�");
        moveAbility.FloatField("�ƶ���");
    }
}