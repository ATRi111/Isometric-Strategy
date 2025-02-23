using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ChargeAttackSkill))]
public class ChargeAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty damageAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        damageAmplifier.FloatField("ÉËº¦Ôö·ù");
    }
}