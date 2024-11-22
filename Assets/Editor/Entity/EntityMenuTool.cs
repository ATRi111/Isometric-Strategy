using MyTool;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EntityMenuTool
{
    [MenuItem("Tools/Game/重新生成所有词条")]
    public static void CreateAllPropertyModifiers()
    {
        string path = Application.dataPath + "/ScriptableObjects/PropertyModifier";
        GeneralTool.DeleteAllFiles(path);
        AssetDatabase.Refresh();
        List<Type> types = GeneralTool.GetSubclasses(typeof(FindPawnPropertySO));
        for (int i = 0; i < types.Count; i++)
            GeneralTool.CreateScriptableObject(types[i], "Assets/ScriptableObjects/PropertyModifier", false);
        AssetDatabase.SaveAssets();
    }
}
