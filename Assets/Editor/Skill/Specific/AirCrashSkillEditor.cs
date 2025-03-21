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
        powerAmplifier.FloatField("ÿ���˺�����");
        speedMultiplier.FloatField("�����ٶ�");
    }
}