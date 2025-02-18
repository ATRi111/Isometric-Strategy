using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Entity))]
public class EntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty entityName, description;

    protected override void MyOnInspectorGUI()
    {
        entityName.TextField("¸öÌåÃû");
        description.TextArea("ÃèÊö");
    }
}