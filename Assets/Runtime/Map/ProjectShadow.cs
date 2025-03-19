using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class ProjectShadow : MonoBehaviour
{
    private IsometricGridManager igm;
    private ShadowManager shadowManager;
    private SpriteRenderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        igm = GetComponentInParent<IsometricGridManager>();
        if(shadowManager != null)
        {
            float radiance = shadowManager.GetProjectRadiance();
            myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
        }
    }

    private void FixedUpdate()
    {
        Vector3 cell = IsometricGridUtility.WorldToCell(transform.parent.position, igm.Grid.cellSize);
        Vector2Int xy = new(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
        int layer = igm.AboveGroundLayer(xy);
        cell = new Vector3(cell.x, cell.y, layer);
        transform.position = IsometricGridUtility.CellToWorld(cell, igm.Grid.cellSize);
    }
}
