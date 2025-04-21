using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO), true)]
public class BuffSOEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty duration, superimposeMode, buffType, unremovable, primitiveValue, modifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        duration.IntField("持续时间");
        superimposeMode.EnumField<ESuperimposeMode>("叠加方式");
        buffType.EnumField<EBuffType>("Buff类型");
        unremovable.BoolField("无法被移除");
        primitiveValue.FloatField("对被施加者的价值");
        modifier.PropertyField("价值计算方式");
    }
}