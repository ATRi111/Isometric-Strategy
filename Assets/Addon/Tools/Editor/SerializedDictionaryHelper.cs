using System;
using System.Collections.Generic;
using UnityEditor;

namespace MyTool
{
    public static class SerializedDictionaryHelper
    {
        /// <summary>
        /// �Զ���ȫSerializedDictionary�е�Ԫ�أ�ʹ֮��ĳ��Enumƥ��
        /// </summary>
        /// <param name="property">SerializedDictionary�е�list��Ӧ��SerializedProperty</param>
        public static void FixEnum<TKey>(SerializedProperty property) where TKey : Enum
        {
            HashSet<TKey> temp = new();
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                SerializedProperty key = element.FindPropertyRelative(nameof(key));
                temp.Add((TKey)Enum.ToObject(typeof(TKey), key.enumValueIndex));
            }
            Array array = Enum.GetValues(typeof(TKey));
            foreach (TKey e in array)
            {
                if (!temp.Contains(e))
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    SerializedProperty element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    SerializedProperty key = element.FindPropertyRelative(nameof(key));
                    key.enumValueIndex = e.GetHashCode();
                }
            }
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}