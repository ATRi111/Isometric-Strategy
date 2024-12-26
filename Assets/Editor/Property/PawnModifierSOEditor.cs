using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnModifierSO), true)]
public class PawnModifierSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, skillsAttached, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("属性词条");
        skillsAttached.ListField("附带技能");
        extraDescription.TextArea("额外描述");
        EditorGUILayout.LabelField("描述");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as PawnModifierSO).Description);
        EditorGUI.EndDisabledGroup();
    }
}