using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AnimationManager))]
public class AnimationManagerEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty inspectorAnimations;

    protected override void MyOnInspectorGUI()
    {
        if (Application.isPlaying)
        {
            EditorGUI.BeginDisabledGroup(true);
            inspectorAnimations.ListField("��ǰ����");
            EditorGUI.EndDisabledGroup();
        }
    }
}