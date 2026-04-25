using Services.ObjectPools;
using UnityEngine;

public class GroundShaderController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool visible;
    public Vector3 shadowMapCoord;
    private MaterialPropertyBlock groundMbp;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        groundMbp = new MaterialPropertyBlock();
    }

    private void Start()
    {
        if (!visible)
        {
            spriteRenderer.enabled = false;
            enabled = false;
            return;
        }

        Vector3Int cellPosition = IsometricGridManager.Instance.WorldToCell(transform.position);
        Vector4 cell = new(cellPosition.x, cellPosition.y, cellPosition.z, 1);

        spriteRenderer.GetPropertyBlock(groundMbp);
        groundMbp.SetVector("_CellPosition", new Vector4(cell.x, cell.y, cell.z, 1f));
        spriteRenderer.SetPropertyBlock(groundMbp);
        //spriteRenderer.material.SetVector("_CellPosition", cell);
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
