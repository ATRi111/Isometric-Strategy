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
            sortingOrderThreshold.IntField("͸����ֵ");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("����"))
            {
                igm.ModifySortingOrderThreshold(-10);
            }
            if (GUILayout.Button("��ǳ"))
            {
                igm.ModifySortingOrderThreshold(+10);
            }
            if (GUILayout.Button("��ԭ"))
            {
                igm.ResetSortingOrderThreshold();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("��������"))
            {
                igm.MoveUp();
            }
            if (GUILayout.Button("��������"))
            {
                igm.MoveDown();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}