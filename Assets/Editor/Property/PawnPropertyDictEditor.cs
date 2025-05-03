using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PawnPropertyDict))]
public class PawnPropertyDictEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty properties;

    protected override void OnEnable()
    {
        base.OnEnable();
        appendInspector = true;
    }

    protected override void MyOnInspectorGUI()
    {
        if (GUILayout.Button("�Զ���ȡ"))
        {
            properties.ClearArray();
            string assetPath = AssetDatabase.GetAssetPath(target);              //�ļ�·��
            string directoryPath = System.IO.Path.GetDirectoryName(assetPath);  //�����ļ�����·��

            string[] guids = AssetDatabase.FindAssets("t:FindPawnPropertySO", new[] { directoryPath });
            int count = 0;
            foreach (string guid in guids)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(filePath);
                FindPawnPropertySO so = obj as FindPawnPropertySO;
                if (so != null)
                {
                    properties.InsertArrayElementAtIndex(count);
                    SerializedProperty element = properties.GetArrayElementAtIndex(count);
                    element.objectReferenceValue = so;
                    count++;
                }
            }
        }
    }
}