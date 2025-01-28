using EditorExtend;
using MyTool;
using UnityEditor;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, targetToKill, pClass, race, actionTime, speedUpRate, time, hidden;

    public bool foldout;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("阵营");
        if ((EFaction)faction.enumValueIndex == EFaction.Enemy)
            targetToKill.BoolField("任务目标");
        else
            targetToKill.boolValue = false;
        pClass.PropertyField("职业");
        race.PropertyField("种族");
        actionTime.PropertyField("基础行动时间");
        speedUpRate.PropertyField("加速率");
        time.IntField("累积等待时间");
        hidden.BoolField("不可见角色");
        foldout = AutoDictionaryDrawerHelper.OnInspectorGUI(foldout, "角色参数", (target as PawnEntity).parameterDict.dict);
    }
}