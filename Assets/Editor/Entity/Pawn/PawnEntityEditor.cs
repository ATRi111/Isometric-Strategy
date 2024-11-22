using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pClass, race, actionTime, time;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("阵营");
        pClass.PropertyField("职业");
        race.PropertyField("种族");
        actionTime.PropertyField("基础行动时间");
        time.IntField("累积等待时间");
    }
}