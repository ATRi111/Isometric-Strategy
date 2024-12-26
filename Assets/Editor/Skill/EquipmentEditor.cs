using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slotType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slotType.EnumField<ESlotType>("≤€Œª¿‡–Õ");
    }
}