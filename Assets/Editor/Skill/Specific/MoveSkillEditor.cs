using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(MoveSkill), true)]
public class MoveSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty actionTimePerUnit, ZOCActionTime, speedMultiplier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        actionTimePerUnit.IntField("每格时间消耗");
        ZOCActionTime.IntField("ZOC时间消耗");
        speedMultiplier.FloatField("动画速度");
    }
}