using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(SummonSkill))]
public class SummonSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty prefab;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        prefab.PropertyField("’ŸªΩŒÔ");
    }
}