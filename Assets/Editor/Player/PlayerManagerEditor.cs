using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty playerDataList, unusedEquipmentList, isSelected;

    protected override void MyOnInspectorGUI()
    {
        playerDataList.ListField("角色数据");
        unusedEquipmentList.ListField("未使用装备");
        isSelected.ListField("出战角色");
    }
}