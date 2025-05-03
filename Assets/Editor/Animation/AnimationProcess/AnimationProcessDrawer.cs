using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AnimationProcess))]
public class AnimationProcessDrawer : AutoPropertyDrawer
{
    [AutoProperty]
    public SerializedProperty name, completed, joinedAnimation;

    protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        name.TextField("Ãû³Æ", NextRectRelative());
        completed.BoolField("²¥·ÅÍê±Ï", NextRectRelative());
    }
}