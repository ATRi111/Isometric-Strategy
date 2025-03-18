using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    public Vector3Int cellNormal;

    private ShadowManager shadowManager;
    private GridObject gridObject;
    private SpriteRenderer myRenderer;
    private ShadowVertex vertex;

    public int height = 1;

    private void Awake()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        gridObject = GetComponentInParent<GridObject>();
        vertex = new(gridObject.CellPosition, cellNormal);
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(!shadowManager.VisibleCheck(vertex))
        {
            Destroy(gameObject);
        }
        else
        {
            float radiance = shadowManager.GetVisibility(vertex);
            myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
        }
    }

#if UNITY_EDITOR
    public void UpdateColor(ShadowManager shadowManager)
    {
        myRenderer = GetComponent<SpriteRenderer>();
        gridObject = GetComponentInParent<GridObject>();
        vertex = new(gridObject.CellPosition, cellNormal);
        float radiance = shadowManager.AmbientOcculasion(vertex);
        myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
    }

    public void ResetColor()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.color = myRenderer.color.SetAlpha(0f);
    }

#endif
}
