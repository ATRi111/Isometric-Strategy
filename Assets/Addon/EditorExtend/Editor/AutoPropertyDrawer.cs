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
        protected bool foldout;
        protected Vector2 min;
        protected float width;
        protected float sum;

        public Rect NextRect(float multiplier = 1)
        {
            Rect ret = new(min, new Vector2(width, EditorGUIUtility.singleLineHeight * multiplier));
            min.y += EditorGUIUtility.singleLineHeight * multiplier;
            sum += multiplier;
            return ret;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return sum * EditorGUIUtility.singleLineHeight;
        }

        public virtual void Initialize(Rect position, SerializedProperty property)
        {
            min = position.min;
            width = position.width;
            sum = 0;
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

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Initialize(position,property);
            //FoldoutHeaderGroup不能嵌套，这里仅仅是模仿嵌套的视觉效果
            EditorGUI.BeginProperty(position, label, property);
            foldout = EditorGUI.Foldout(NextRect(), foldout, label);
            if (foldout)
            {
                EditorGUI.indentLevel++;
                MyOnGUI(position, property, label);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// 此方法中禁止调用Layout版本的EditorGUI,必须使用NextRect
        /// </summary>
        protected abstract void MyOnGUI(Rect position, SerializedProperty property, GUIContent label);
    }
}