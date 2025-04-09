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
        name.TextField("参数名", NextRectRelative());
        icon.PropertyField("图标",NextRectRelative());
        maxValue.IntField("上限", NextRectRelative());
        valuePerUnit.IntField("单位价值", NextRectRelative());
        hidden.BoolField("隐藏参数", NextRectRelative());
        description.TextField("描述", NextRectRelative());
    }
}