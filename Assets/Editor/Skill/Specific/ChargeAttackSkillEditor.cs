using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ChargeAttackSkill))]
public class ChargeAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty powerAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        powerAmplifier.FloatField("ÉËº¦Ôö·ù");
    }
}