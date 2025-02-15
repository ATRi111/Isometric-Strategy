using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RangedSkill), true)]
public class RangedSkillEditor : AimSkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance, aimAtSelf;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField("施放距离");
        aimAtSelf.BoolField("可在自身位置施放");
    }
}