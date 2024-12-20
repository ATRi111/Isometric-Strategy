using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PawnParameterModifier))]
public class PawnParameterModifierDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty parameterName, deltaValue;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        parameterName.TextField("������", NextRectRelative());
        deltaValue.IntField("�ı���", NextRectRelative());
    }
}