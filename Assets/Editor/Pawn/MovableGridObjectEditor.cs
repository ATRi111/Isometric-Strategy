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
        climbAbility.IntField("攀爬力");
        dropAbility.IntField("下落力");
        moveAbility.IntField("移动力");
    }
}