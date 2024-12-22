using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EquipmentSlot))]
public class EquipmentSlotDrawer : AutoPropertyDrawer
{
    public override bool NoLabel => true;

    [AutoProperty]
    public SerializedProperty slotType, equipment;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        slotType.EnumField<ESlotType>("��λ����", NextRectRelative());
        AutoPropertyField("װ��", equipment);
    }
}