using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(GridSurface))]
public class GridSurfaceEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty difficulty, leftLadder, rightLadder;

    protected override void MyOnInspectorGUI()
    {
        difficulty.IntField("�ƶ��Ѷ�");
        leftLadder.BoolField("�������");
        rightLadder.BoolField("�Ҳ�����");
    }
}