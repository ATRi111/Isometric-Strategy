using EditorExtend;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty slotType, animationName, parameterOnAgent;

    protected List<string> animationNames;
    protected override void OnEnable()
    {
        base.OnEnable();
        animationNames = new();
        List<GameObject> temp = new();
        EditorExtendUtility.FindAssets($"武器动画 t:GameObject", temp);
        for (int i = 0; i < temp.Count; i++)
        {
            animationNames.Add(temp[i].name);
        }
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        slotType.EnumField<ESlotType>("槽位类型");
        animationName.TextFieldWithOptionButton("动画预制体名", animationNames);
        parameterOnAgent.ListField("初始参数");
    }
}