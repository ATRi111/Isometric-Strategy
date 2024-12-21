using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO))]
public class BuffSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, duration, superimposeMode, primitiveValue;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("属性词条");
        duration.IntField("持续时间");
        superimposeMode.EnumField<ESuperimposeMode>("叠加方式");
        primitiveValue.FloatField("价值");
    }
}