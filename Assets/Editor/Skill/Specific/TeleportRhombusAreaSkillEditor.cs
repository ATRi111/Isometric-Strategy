using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(TeleportRhombusAreaSkill))]
public class TeleportRhombusAreaSkillEditor : RhombusAreaSkillEditor
{
    [AutoProperty]
    public SerializedProperty powerAmplifier, speedMultiplier, jumpHeight;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        powerAmplifier.FloatField("ÿ���˺�����");
        speedMultiplier.FloatField("�����ٶ�");
        jumpHeight.FloatField("���������߶�");
    }
}