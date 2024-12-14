using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(BattleComponent))]
public class HealthComponentEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty maxHP, hp, resistance;
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
        list.PropertyField("抗性");
    }
}