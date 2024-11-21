using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pawnClass, pawnRace, actionTime, time;

    protected override void MyOnInspectorGUI()
    {
        faction.EnumField<EFaction>("阵营");
        pawnClass.PropertyField("职业");
        pawnRace.PropertyField("种族");
        actionTime.IntField("基础行动时间");
        time.IntField("累积等待时间");
    }
}