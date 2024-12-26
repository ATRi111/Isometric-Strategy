using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnClass))]
public class PawnClassEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty supportAbility, offenseAbility, bestSupprtDistance, bestOffenseDistance;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        supportAbility.FloatField("Э������");
        offenseAbility.FloatField("��в����");
        bestSupprtDistance.IntField("���Э������");
        bestOffenseDistance.IntField("�����в����");
    }
}