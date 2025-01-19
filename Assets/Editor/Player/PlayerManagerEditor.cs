using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty playerDataList, unusedEquipmentList, isSelected;

    protected override void MyOnInspectorGUI()
    {
        playerDataList.ListField("��ɫ����");
        unusedEquipmentList.ListField("δʹ��װ��");
        isSelected.ListField("��ս��ɫ");
    }
}