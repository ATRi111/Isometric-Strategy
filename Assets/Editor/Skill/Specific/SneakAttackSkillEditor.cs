using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(SneakAttackSkill))]
public class SneakAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty damageAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        damageAmplifier.FloatField("±³´ÌÉËº¦Ôö·ù");
    }
}