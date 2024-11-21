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
        climbAbility.PropertyField("攀爬力");
        dropAbility.PropertyField("下落力");
        moveAbility.PropertyField("移动力");
    }
}