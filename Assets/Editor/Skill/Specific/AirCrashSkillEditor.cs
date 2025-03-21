using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AirCrashSkill))]
public class AirCrashSkillEditor : RhombusAreaSkillEditor
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