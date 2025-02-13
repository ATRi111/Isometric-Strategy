using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(HealthAbsorbSkill))]
public class HealthAbsorbSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty absorbPercent;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        absorbPercent.FloatField("ÎüÈ¡±ÈÀý");
    }
}