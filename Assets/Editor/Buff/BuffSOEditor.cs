using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO), true)]
public class BuffSOEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty duration, superimposeMode, primitiveValue;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        duration.IntField("持续时间");
        superimposeMode.EnumField<ESuperimposeMode>("叠加方式");
        primitiveValue.FloatField("对友方的价值");
    }
}