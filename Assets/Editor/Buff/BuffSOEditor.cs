using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO))]
public class BuffSOEditor : PawnPropertyModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty duration, superimposeMode, primitiveValue;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        duration.IntField("����ʱ��");
        superimposeMode.EnumField<ESuperimposeMode>("���ӷ�ʽ");
        primitiveValue.FloatField("���ѷ��ļ�ֵ");
    }
}