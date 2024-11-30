using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(Skill), true)]
public class SkillEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty displayName, actionTime, accuracy, powers;

    protected override void MyOnInspectorGUI()
    {
        displayName.TextField("展示技能名");
        actionTime.IntField("行动时间");
        accuracy.IntSlider("命中率", 0, Skill.MaxAccuracy);
        powers.ListField("威力");
    }
}