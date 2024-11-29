using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RangedSkill), true)]
public class RangedSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance, victimType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField("��������");
        victimType.EnumField<EVictimType>("Ŀ������");
    }
}