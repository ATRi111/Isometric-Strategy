using EditorExtend;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slotType, animationName, parameterOnAgent;

    protected List<string> animationNames;
    protected override void OnEnable()
    {
        base.OnEnable();
        animationNames = EditorExtendUtility.FindAssetsNames($"�������� t:GameObject");
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slotType.EnumField<ESlotType>("��λ����");
        animationName.TextFieldWithOptionButton("����Ԥ������", animationNames);
        parameterOnAgent.ListField("��ʼ����");
    }
}