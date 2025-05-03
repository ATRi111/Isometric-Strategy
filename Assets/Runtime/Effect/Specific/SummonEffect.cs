using EditorExtend.GridEditor;
using Services;
using System.Text;
using UnityEngine;

public class SummonEffect : Effect
{
    public GameObject prefab;
    public Vector3Int cellPosition;
    private GameObject summoned;

    public SummonEffect(PawnEntity agent, GameObject prefab, Vector3Int cellPosition, int probability = 100)
        : base(agent, probability)
    {
        this.prefab = prefab;
        this.cellPosition = cellPosition;
    }

    public override bool Appliable => summoned == null;

    public override void Apply()
    {
        base.Apply();
        summoned = Object.Instantiate(prefab);
        summoned.SetActive(false);
        summoned.transform.SetParent(victim.transform.parent);
        GridObject gridObject = summoned.GetComponent<GridObject>();
        gridObject.CellPosition = cellPosition;
        summoned.SetActive(true);
        PawnEntity pawnEntity = summoned.GetComponent<PawnEntity>();
        pawnEntity.time = ServiceLocator.Get<GameManager>().Time;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        //TODO:召唤物价值
        return 10000f;
    }

    public override void Describe(StringBuilder sb, bool result)
    {
        if (!result && !AlwaysHappen)
        {
            sb.Append(probability);
            sb.Append("%");
        }
        sb.Append("在");
        sb.Append(cellPosition);
        sb.Append("处召唤一个");
        sb.Append(prefab.GetComponent<PawnEntity>().EntityName);
    }
}
