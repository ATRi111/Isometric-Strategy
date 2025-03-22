using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class ProjectShadow : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private ShadowManager shadowManager;
    private SpriteRenderer myRenderer;

    private void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        if(shadowManager != null)
        {
            float radiance = shadowManager.GetProjectRadiance();
            myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
        }
    }

    private void FixedUpdate()
    {
        Vector3 cell = IsometricGridUtility.WorldToCell(transform.parent.position, Igm.Grid.cellSize);
        Vector2Int xy = new(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
        int layer = Igm.AboveGroundLayer(xy);
        cell = new Vector3(cell.x, cell.y, layer);
        transform.position = IsometricGridUtility.CellToWorld(cell, Igm.Grid.cellSize);
    }
}
