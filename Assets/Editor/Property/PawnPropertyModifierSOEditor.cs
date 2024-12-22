using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnPropertyModifierSO), true)]
public class PawnPropertyModifierSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, skillsAttached;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("属性词条");
        skillsAttached.ListField("附带技能");
    }
}