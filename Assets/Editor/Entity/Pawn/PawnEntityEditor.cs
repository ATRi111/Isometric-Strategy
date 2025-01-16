using EditorExtend;
using MyTool;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PawnEntity))]
public class PawnEntityEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty faction, pClass, race, actionTime, speedUpRate, time;

    public bool foldout;

    protected override void MyOnInspectorGUI()
    {   
        faction.EnumField<EFaction>("阵营");
        pClass.PropertyField("职业");
        race.PropertyField("种族");
        actionTime.PropertyField("基础行动时间");
        speedUpRate.PropertyField("加速率");
        if (Application.isPlaying)
            time.IntField("累积等待时间");
        foldout = AutoDictionaryDrawerHelper.OnInspectorGUI(foldout, "角色参数", (target as PawnEntity).parameterDict.dict);
    }
}