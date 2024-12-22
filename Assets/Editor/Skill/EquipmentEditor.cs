using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnPropertyModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slotType, skillsAttached;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slotType.EnumField<ESlotType>("槽位类型");
        skillsAttached.ListField("附带技能");
    }
}