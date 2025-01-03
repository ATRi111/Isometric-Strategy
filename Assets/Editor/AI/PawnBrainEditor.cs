using EditorExtend;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PawnBrain))]
public class PawnBrainEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty humanControl, plans, prepared, learnedSkills;

    protected override void MyOnInspectorGUI()
    {
        humanControl.BoolField("由玩家控制");
        learnedSkills.PropertyField("技能");
        if(Application.isPlaying)
        {
            plans.ListField("当前计划");
            prepared.BoolField("准备好行动");
        }
    }
}