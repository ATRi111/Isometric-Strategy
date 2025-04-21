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
        duration.IntField("����ʱ��");
        superimposeMode.EnumField<ESuperimposeMode>("���ӷ�ʽ");
        buffType.EnumField<EBuffType>("Buff����");
        unremovable.BoolField("�޷����Ƴ�");
        primitiveValue.FloatField("�Ա�ʩ���ߵļ�ֵ");
        modifier.PropertyField("��ֵ���㷽ʽ");
    }
}