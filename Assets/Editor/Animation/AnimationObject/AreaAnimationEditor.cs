using EditorExtend;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AreaAnimation))]
public class AreaAnimationEditor : AnimationObjectEditor
{
    [AutoProperty]
    public SerializedProperty prefabName;

    protected List<string> prefabNames;

    protected override void OnEnable()
    {
        base.OnEnable();
        prefabNames = EditorExtendUtility.FindAssetsNames($"������Ч������ t:GameObject");
    }

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        prefabName.TextFieldWithOptionButton("������", prefabNames);
    }
}