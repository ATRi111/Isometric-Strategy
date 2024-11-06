using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    /// <summary>
    /// 通常，继承此类后只需要重写MyOnGUI，并在其中使用Layout版本的GUI即可，不需要重写其他函数
    /// </summary>
    public abstract class AutoPropertyDrawer : PropertyDrawer
    {
        private bool foldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            Initialize(property);
            //FoldoutHeaderGroup不能嵌套，这里仅仅是模仿嵌套的视觉效果
            foldout = EditorGUI.Foldout(position, foldout, label);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                MyOnGUI(position, property, label);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }

        protected abstract void MyOnGUI(Rect position, SerializedProperty property, GUIContent label);

        public virtual void Initialize(SerializedProperty property)
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo info in fields)
            {
                if (AutoPropertyAttribute.TryGetPropertyName(info, out string name))
                {
                    SerializedProperty temp = property.FindPropertyRelative(name);
                    if (temp != null)
                        info.SetValue(this, property.FindPropertyRelative(name));
                    else
                        Debug.Log($"{property.name}中找不到名为{name}的字段");
                }
            }
        }
    }
}