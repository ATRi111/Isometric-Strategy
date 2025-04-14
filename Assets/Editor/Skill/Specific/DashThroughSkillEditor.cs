using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(DashThroughSkill))]
public class DashThroughSkillEditor : ChargeAttackSkillEditor
{
    [AutoProperty]
    public SerializedProperty speedMultiplier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        speedMultiplier.FloatField("¶¯»­ËÙ¶È");
    }
}