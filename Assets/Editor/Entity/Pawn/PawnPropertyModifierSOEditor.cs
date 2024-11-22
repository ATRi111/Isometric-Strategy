using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnPropertyModifierSO))]
public class PawnPropertyModifierSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("¥ Ãı");
    }
}