using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(ProjectileSkill), true)]
public class ProjectileSkillEditor : RangedSkillEditor
{
    [AutoProperty]
    public SerializedProperty speedMultiplier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        speedMultiplier.FloatField("动画速度倍率");
    }
}