using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(OffenceComponent))]
public class OffenceComponentEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty strength, dexterity, intelligence;

    protected override void MyOnInspectorGUI()
    {
        strength.PropertyField("¡¶¡ø");
        dexterity.PropertyField("¡È«…");
        intelligence.PropertyField("÷«¡¶");
    }
}