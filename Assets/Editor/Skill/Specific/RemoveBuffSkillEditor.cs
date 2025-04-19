using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RemoveBuffSkill))]
public class RemoveBuffSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty buffType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        buffType.EnumField<EBuffType>("ø…“∆≥˝Buff¿‡–Õ");
    }
}