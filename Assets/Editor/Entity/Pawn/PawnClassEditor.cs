using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(PawnClass))]
public class PawnClassEditor : PawnModifierSOEditor
{
    [AutoProperty]
    public SerializedProperty supportAbility, offenseAbility, bestSupprtDistance, bestOffenseDistance, terrainAbility;

    protected override void MyOnInspectorGUI()
    {
        base.MyOnInspectorGUI();
        supportAbility.FloatField("Э������");
        offenseAbility.FloatField("��в����");
        terrainAbility.FloatField("������������");
        bestSupprtDistance.IntField("���Э������");
        bestOffenseDistance.IntField("�����в����");
    }
}