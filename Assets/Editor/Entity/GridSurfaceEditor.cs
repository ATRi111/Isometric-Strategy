using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(GridSurface))]
public class GridSurfaceEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty difficulty, leftLadder, rightLadder;

    protected override void MyOnInspectorGUI()
    {
        difficulty.IntField("移动难度");
        leftLadder.BoolField("左侧梯子");
        rightLadder.BoolField("右侧梯子");
    }
}