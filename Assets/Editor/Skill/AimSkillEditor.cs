using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill), true)]
public class AimSkillEditor : SkillEditor
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