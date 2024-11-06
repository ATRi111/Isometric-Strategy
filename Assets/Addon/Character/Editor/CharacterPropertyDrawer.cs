using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace Character
{
    public abstract class CharacterPropertyDrawer : AutoPropertyDrawer
    {
        [AutoProperty]
        public SerializedProperty defaultValue, currentValue;
    }

    [CustomPropertyDrawer(typeof(IntProperty))]
    public class IntPropertyDrawer : CharacterPropertyDrawer
    {
        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            defaultValue.IntField("Ĭ��ֵ");
            EditorGUI.BeginDisabledGroup(true);
            currentValue.IntField("��ǰֵ");
            EditorGUI.EndDisabledGroup();
        }
    }

    [CustomPropertyDrawer(typeof(FloatProperty))]
    public class FloatPropertyDrawer : CharacterPropertyDrawer
    {
        protected override void MyOnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            defaultValue.FloatField("Ĭ��ֵ");
            EditorGUI.BeginDisabledGroup(true);
            currentValue.FloatField("��ǰֵ");
            EditorGUI.EndDisabledGroup();
        }
    }
}