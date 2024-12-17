using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill), true)]
public class AimSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty powers, accuracy, minLayer, maxLayer;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        accuracy.IntSlider("������", 0, AimSkill.MaxAccuracy);
        powers.ListField("����");
        EditorExtendGUI.IntMinMaxSlider(minLayer, maxLayer, "ʩ�Ÿ߶ȲΧ", -32, 32);
        EditorGUILayout.BeginHorizontal();
        minLayer.IntField("");
        maxLayer.IntField("");
        EditorGUILayout.EndHorizontal();
    }
}