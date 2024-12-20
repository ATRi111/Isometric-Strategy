using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PawnParameterModifier))]
public class PawnParameterModifierDrawer : AutoPropertyDrawer
{
    public static int[] optionValues;
    public static string[] displayOptions;

    static PawnParameterModifierDrawer()
    {
        int n = PawnEntity.ParameterTable.parameters.Count;
        optionValues = new int[n];
        displayOptions = new string[n];
        for (int i = 0; i < n; i++)
        {
            optionValues[i] = i;
            displayOptions[i] = PawnEntity.ParameterTable.parameters[i].name;
        }
    }

    public static void ParameterIndexField(SerializedProperty property, Rect rect)
    {
        property.IntPopField("参数名", displayOptions, optionValues, rect);
    }

    [AutoProperty]
    public SerializedProperty parameterIndex, deltaValue;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ParameterIndexField(parameterIndex, NextRectRelative());
        deltaValue.IntField("改变量", NextRectRelative());
    }
}