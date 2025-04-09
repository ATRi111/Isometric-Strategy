using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Parameter))]
public class ParameterDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty name, maxValue, valuePerUnit, hidden, description, icon;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        name.TextField("������", NextRectRelative());
        icon.PropertyField("ͼ��",NextRectRelative());
        maxValue.IntField("����", NextRectRelative());
        valuePerUnit.IntField("��λ��ֵ", NextRectRelative());
        hidden.BoolField("���ز���", NextRectRelative());
        description.TextField("����", NextRectRelative());
    }
}