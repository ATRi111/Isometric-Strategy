using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(DefenceComponent))]
public class DefenceComponentEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty maxHP, hp, damageMultiplier, resistance;
    public SerializedProperty list;

    protected override void OnEnable()
    {
        base.OnEnable();
        list = resistance.FindPropertyRelative(nameof(list));
        SerializedDictionaryHelper.FixEnum<EDamageType>(list);
    }

    protected override void MyOnInspectorGUI()
    {
        maxHP.PropertyField("最大生命");
        hp.IntField("当前生命");
        damageMultiplier.PropertyField("承伤系数");
        list.PropertyField("抗性");
    }
}