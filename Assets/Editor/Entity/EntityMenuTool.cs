using MyTool;
using System;
using System.Collections.Generic;
using UnityEditor;

public static class EntityMenuTool
{
    [MenuItem("Tools/Game/自动生成词条")]
    public static void CreateAllPropertyModifiers()
    {
        AssetDatabase.Refresh();
        List<Type> types = GeneralTool.GetSubclasses(typeof(FindPawnPropertySO));
        for (int i = 0; i < types.Count; i++)
            GeneralTool.CreateScriptableObject(types[i], "Assets/ScriptableObjects/PropertyModifier");
    }
}
