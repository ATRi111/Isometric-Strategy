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
        if (GUILayout.Button("自动获取"))
        {
            properties.ClearArray();
            string assetPath = AssetDatabase.GetAssetPath(target);              //文件路径
            string directoryPath = System.IO.Path.GetDirectoryName(assetPath);  //不含文件名的路径

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