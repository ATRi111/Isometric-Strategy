using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(FindPawnPropertySO),true)]
public class FindPawnPropertySOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty negative, description;

    protected override void MyOnInspectorGUI()
    {
        negative.BoolField("��������");
        description.TextField("����");
    }
}