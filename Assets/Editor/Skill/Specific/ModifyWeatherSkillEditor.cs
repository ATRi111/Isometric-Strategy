using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ModifyWeatherSkill))]
public class ModifyWeatherSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty weatherModifiers;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        weatherModifiers.PropertyField("ÌìÆø±í");
    }
}