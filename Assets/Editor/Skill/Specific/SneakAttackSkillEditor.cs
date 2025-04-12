using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(SneakAttackSkill))]
public class SneakAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty damageAmplifier, ignorePierceResistance;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        damageAmplifier.FloatField("背刺伤害增幅");
        ignorePierceResistance.IntField("无视护甲");
    }
}