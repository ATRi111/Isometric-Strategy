using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RangedSkill), true)]
public class RangedSkillEditor : AimSkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance, victimType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField("施放距离");
        victimType.EnumField<EVictimType>("目标类型");
    }
}