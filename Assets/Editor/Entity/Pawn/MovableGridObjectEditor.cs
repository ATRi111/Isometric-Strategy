using EditorExtend;
using EditorExtend.GridEditor;
using UnityEditor;

[CustomEditor(typeof(MovableGridObject))]
public class MovableGridObjectEditor : GridObjectEditor
{
    [AutoProperty]
    public SerializedProperty climbAbility, dropAbility, moveAbility;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        climbAbility.PropertyField("������");
        dropAbility.PropertyField("������");
        moveAbility.PropertyField("�ƶ���");
    }
}