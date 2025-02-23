using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RelativeHeightSkill))]
public class RelativeHeightSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty higher, maxAmplifier, damageAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        higher.BoolField("比目标高时提高伤害");
        damageAmplifier.FloatField("每格伤害增幅");
        maxAmplifier.FloatField("伤害增幅上限");
    }
}