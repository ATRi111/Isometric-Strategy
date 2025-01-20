using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(RangedSkill), true)]
public class RangedSkillEditor : AimSkillEditor
{
    [AutoProperty]
    public SerializedProperty castingDistance;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        castingDistance.IntField(" ©∑≈æ‡¿Î");
    }
}