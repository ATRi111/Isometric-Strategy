using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill), true)]
public class AimSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty powers, buffOnVictim, hitBackProbability, minLayer, maxLayer, victimType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        victimType.EnumField<EVictimType>("目标类型");
        powers.ListField("威力");
        buffOnVictim.ListField("对目标施加的Buff");
        hitBackProbability.IntSlider("击退概率", 0, 100);
        EditorExtendGUI.IntMinMaxSlider(minLayer, maxLayer, "施放高度差范围", -32, 32);
        EditorGUILayout.BeginHorizontal();
        minLayer.IntField("");
        maxLayer.IntField("");
        EditorGUILayout.EndHorizontal();
    }
}