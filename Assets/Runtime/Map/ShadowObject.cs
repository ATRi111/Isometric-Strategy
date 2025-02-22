using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    [Range(0f,1f)]
    public float maxRadiance = 1f;
    public Vector3Int cellNormal;

    private ShadowManager shadowManager;
    private GridObject gridObject;
    private SpriteRenderer upRenderer;
    private ShadowVertex up;

    public int height = 1;

    private void Awake()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        gridObject = GetComponentInParent<GridObject>();
        up = new(gridObject.CellPosition, cellNormal);
        upRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        float visibility = shadowManager.GetVisibility(up);
        float radiance = maxRadiance * visibility;
        upRenderer.color = upRenderer.color.SetAlpha(1f - radiance);
    }
}
