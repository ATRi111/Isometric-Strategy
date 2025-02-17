using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(SneakAttackSkill))]
public class SneakAttackSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty powerAmplifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        powerAmplifier.FloatField("±³´ÌÉËº¦Ôö·ù");
    }
}