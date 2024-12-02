using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill))]
public class OffenseSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty powers,accuracy;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        accuracy.IntSlider("ÃüÖÐÂÊ", 0, AimSkill.MaxAccuracy);
        powers.ListField("ÍþÁ¦");
    }
}