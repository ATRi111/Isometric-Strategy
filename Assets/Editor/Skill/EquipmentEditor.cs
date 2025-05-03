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
        slotType.EnumField<ESlotType>("槽位类型");
        animationPrefab.PropertyField("动画预制体名");
        parameterOnAgent.ListField("初始参数");
    }
}