using EditorExtend;
using EditorExtend.GridEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IsometricGridManager))]
public class IsometricGridManagerEditor : IsometricGridManagerBaseEditor
{
    [AutoProperty]
    public SerializedProperty sortingOrderThreshold;
    private IsometricGridManager igm;

    protected override void OnEnable()
    {
        base.OnEnable();
        igm = target as IsometricGridManager;
        GridManager.AddAllObjects();
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
                igm.ModifySortingOrderThreshold(-10);
            }
            if (GUILayout.Button("更浅"))
            {
                igm.ModifySortingOrderThreshold(+10);
            }
            if (GUILayout.Button("复原"))
            {
                igm.ResetSortingOrderThreshold();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("整体上移"))
            {
                igm.MoveUp();
            }
            if (GUILayout.Button("整体下移"))
            {
                igm.MoveDown();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}