using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO), true)]
public class BuffSOEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty duration, superimposeMode, removeFlag, primitiveValue, modifier;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        duration.IntField("持续时间");
        superimposeMode.EnumField<ESuperimposeMode>("叠加方式");
        removeFlag.EnumField<ERemoveFlag>("移除标志");
        primitiveValue.FloatField("对被施加者的价值");
        modifier.PropertyField("价值计算方式");
    }
}