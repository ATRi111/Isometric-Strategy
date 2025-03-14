using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RemoveBuffSkill))]
public class RemoveBuffSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty removeFlag;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        removeFlag.EnumField<ERemoveFlag>("ø…“∆≥˝Buff¿‡–Õ");
    }
}