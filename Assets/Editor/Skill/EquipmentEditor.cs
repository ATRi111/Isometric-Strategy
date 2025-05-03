using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slotType, animationPrefab, parameterOnAgent;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slotType.EnumField<ESlotType>("��λ����");
        animationPrefab.PropertyField("����Ԥ������");
        parameterOnAgent.ListField("��ʼ����");
    }
}