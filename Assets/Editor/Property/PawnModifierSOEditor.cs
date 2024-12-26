using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnModifierSO), true)]
public class PawnModifierSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, skillsAttached, extraDescription;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("���Դ���");
        skillsAttached.ListField("��������");
        extraDescription.TextArea("��������");
        EditorGUILayout.LabelField("����");
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextArea((target as PawnModifierSO).Description);
        EditorGUI.EndDisabledGroup();
    }
}