using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(OffenceComponent))]
public class OffenceComponentEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty strength, dexterity, intelligence, mind;

    protected override void MyOnInspectorGUI()
    {
        strength.PropertyField("����");
        dexterity.PropertyField("����");
        intelligence.PropertyField("����");
        mind.PropertyField("����");
    }
}