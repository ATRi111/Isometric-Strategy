using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnPropertyModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slot, skillsAttached;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slot.EnumField<EEquipmentSlot>("装备槽");
        skillsAttached.ListField("附带技能");
    }
}