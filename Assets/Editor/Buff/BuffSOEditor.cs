using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(BuffSO))]
public class BuffSOEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty propertyModifier, duration, superimposeMode, primitiveValue;

    protected override void MyOnInspectorGUI()
    {
        propertyModifier.PropertyField("���Դ���");
        duration.IntField("����ʱ��");
        superimposeMode.EnumField<ESuperimposeMode>("���ӷ�ʽ");
        primitiveValue.FloatField("��ֵ");
    }
}