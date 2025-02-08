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
            sortingOrderThreshold.IntField("͸����ֵ");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("����"))
            {
                Igm.ModifySortingOrderThreshold(-10);
            }
            if (GUILayout.Button("��ǳ"))
            {
                Igm.ModifySortingOrderThreshold(+10);
            }
            if (GUILayout.Button("��ԭ"))
            {
                Igm.ResetSortingOrderThreshold();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}