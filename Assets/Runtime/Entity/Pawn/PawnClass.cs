using UnityEngine;

[CreateAssetMenu(fileName = "��ְҵ", menuName = "ְҵ")]
public class PawnClass : PawnModifierSO
{
    public float supportAbility;
    public float offenseAbility;
    public int bestSupprtDistance;
    public int bestOffenseDistance;
    public float terrainCoefficient;

    protected override string TypeName => "ְҵ";
}