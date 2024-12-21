using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill), true)]
public class AimSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty powers, buffOnVictim, accuracy, minLayer, maxLayer;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        accuracy.IntSlider("命中率", 0, AimSkill.MaxAccuracy);
        powers.ListField("威力");
        buffOnVictim.ListField("对目标施加的Buff");
        EditorExtendGUI.IntMinMaxSlider(minLayer, maxLayer, "施放高度差范围", -32, 32);
        EditorGUILayout.BeginHorizontal();
        minLayer.IntField("");
        maxLayer.IntField("");
        EditorGUILayout.EndHorizontal();
    }
}