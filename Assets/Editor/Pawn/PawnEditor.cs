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
        faction.EnumField<EFaction>("阵营");
        climbAbility.IntField("最大攀爬高度");
        dropAbility.IntField("最大下落高度");
        moveAbility.FloatField("移动力");
    }
}