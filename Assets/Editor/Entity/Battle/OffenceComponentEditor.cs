using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(OffenceComponent))]
public class OffenceComponentEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty strength, dexterity, intelligence, mind;

    protected override void MyOnInspectorGUI()
    {
        strength.PropertyField("力量");
        dexterity.PropertyField("灵巧");
        intelligence.PropertyField("智力");
        mind.PropertyField("精神");
    }
}