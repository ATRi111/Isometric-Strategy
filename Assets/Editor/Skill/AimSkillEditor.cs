using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(AimSkill), true)]
public class AimSkillEditor : SkillEditor
{
    [AutoProperty]
    public SerializedProperty powers, buffOnVictim, minLayer, maxLayer, victimType;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        victimType.EnumField<EVictimType>("Ŀ������");
        powers.ListField("����");
        buffOnVictim.ListField("��Ŀ��ʩ�ӵ�Buff");
        EditorExtendGUI.IntMinMaxSlider(minLayer, maxLayer, "ʩ�Ÿ߶ȲΧ", -32, 32);
        EditorGUILayout.BeginHorizontal();
        minLayer.IntField("");
        maxLayer.IntField("");
        EditorGUILayout.EndHorizontal();
    }
}