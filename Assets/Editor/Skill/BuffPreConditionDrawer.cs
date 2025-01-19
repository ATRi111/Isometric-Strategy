using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffPreCondition))]
public class BuffPreConditionDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty so, requireExist;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AutoPropertyField("Buff", so);
        requireExist.BoolField("ÒªÇó´æÔÚ", NextRectRelative());
    }
}