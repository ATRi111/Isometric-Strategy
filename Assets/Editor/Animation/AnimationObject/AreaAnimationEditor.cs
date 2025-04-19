using EditorExtend;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AreaAnimation))]
public class AreaAnimationEditor : AnimationObjectEditor
{
    [AutoProperty]
    public SerializedProperty prefab;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        prefab.PropertyField("Éú³ÉÎï");
    }
}