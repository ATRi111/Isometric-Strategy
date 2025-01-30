using EditorExtend.GridEditor;
using Services;
using Services.Asset;
using UnityEngine;

public class SummonEffect : Effect
{
    private readonly static IAssetLoader assetLoader;
    static SummonEffect()
    {
        assetLoader = ServiceLocator.Get<IAssetLoader>();
    }

    public GameObject prefab;
    public Vector3Int cellPosition;
    private GameObject summoned;
    private IsometricGridManager igm;

    public SummonEffect(PawnEntity victim, GameObject prefab,Vector3Int cellPosition,IsometricGridManager igm, int probability = 100) 
        : base(victim, probability)
    {
        this.prefab = prefab;
        this.igm = igm;
        this.cellPosition = cellPosition;
    }

    public override bool Appliable => summoned == null;

    public override bool Revokable => summoned != null;

    public override void Apply()
    {
        base.Apply();
        summoned = Object.Instantiate(prefab);
        summoned.SetActive(false);
        summoned.transform.SetParent(victim.transform.parent);
        GridObject gridObject = summoned.GetComponent<GridObject>();
        gridObject.CellPosition = cellPosition;
        summoned.SetActive(true);
    }

    public override void Revoke()
    {
        base.Revoke();
        Object.Destroy(summoned);
        summoned = null;
    }

    public override float ValueFor(PawnEntity pawn)
    {
        return 0;
    }
}
