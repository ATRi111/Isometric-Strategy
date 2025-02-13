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
        duration.IntField("����ʱ��");
        superimposeMode.EnumField<ESuperimposeMode>("���ӷ�ʽ");
        removeFlag.EnumField<ERemoveFlag>("�Ƴ���־");
        primitiveValue.FloatField("�Ա�ʩ���ߵļ�ֵ");
        modifier.PropertyField("��ֵ���㷽ʽ");
    }
}