using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnPropertyModifierSO), true)]
public class PawnPropertyModifierSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, skillsAttached;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("���Դ���");
        skillsAttached.ListField("��������");
    }
}