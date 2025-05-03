using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "新职业", menuName = "职业")]
public class PawnClass : PawnModifierSO
{
    public float supportAbility;
    public float offenseAbility;
    public int bestSupprtDistance;
    public int bestOffenseDistance;
    public float terrainAbility;

    protected override string TypeName => "职业";

    protected override void Describe(StringBuilder sb)
    {

    }
}