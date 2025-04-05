using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(TeleportRhombusAreaSkill))]
public class TeleportRhombusAreaSkillEditor : RhombusAreaSkillEditor
{
    [AutoProperty]
    public SerializedProperty powerAmplifier, speedMultiplier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        powerAmplifier.FloatField("每层伤害增幅");
        speedMultiplier.FloatField("动画速度");
    }
}