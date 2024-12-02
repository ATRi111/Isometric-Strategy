using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RangedSkill), true)]
public class RangedSkillEditor : OffenseSkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance, victimType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField("ʩ�ž���");
        victimType.EnumField<EVictimType>("Ŀ������");
    }
}