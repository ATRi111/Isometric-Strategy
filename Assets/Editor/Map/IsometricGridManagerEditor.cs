using EditorExtend;
using EditorExtend.GridEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IsometricGridManager))]
public class IsometricGridManagerEditor : IsometricGridManagerBaseEditor
{
    [AutoProperty]
    public SerializedProperty sortingOrderThreshold;
    private IsometricGridManager Igm => target as IsometricGridManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(!Application.isPlaying)
            Igm.ResetSortingOrderThreshold();
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        if (!Application.isPlaying)
        {
            sortingOrderThreshold.IntField("透视阈值");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("更深"))
            {
                Igm.ModifySortingOrderThreshold(-10);
            }
            if (GUILayout.Button("更浅"))
            {
                Igm.ModifySortingOrderThreshold(+10);
            }
            if (GUILayout.Button("复原"))
            {
                Igm.ResetSortingOrderThreshold();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}