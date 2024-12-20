using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AreaEffectSkill))]
public class AreaEffectSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty effectRange;

    protected override void MyOnInspectorGUI()
    {
        effectRange.IntField("AOE·¶Î§");
    }
}