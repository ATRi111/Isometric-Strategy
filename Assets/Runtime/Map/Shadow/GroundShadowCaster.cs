using EditorExtend.GridEditor;
using Services.ObjectPools;
using UnityEngine;

public class GroundShadowCaster : MonoBehaviour
{
    private GridObject gridObject;
    private LightManager lightManager;
    private SpriteRenderer spriteRenderer;
    private Material material;
    private Vector3Int cellPosition;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        lightManager = LightManager.Instance;
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        cellPosition = gridObject.CellPosition;
    }

    private void Start()
    {
        if (!lightManager.VisibleCheck(cellPosition))
        {
            spriteRenderer.enabled = false;
            return;
        }
        Vector4 cell = new(cellPosition.x, cellPosition.y, cellPosition.z, 1);
        material.SetVector("_CellPosition", cell);
        Vector4 v = new(lightManager.lightColor.r, lightManager.lightColor.g, lightManager.lightColor.b, 1);
        material.SetVector("_LightColor", v);
        v = lightManager.lightDirection;
        material.SetVector("_LightDirection", v);
        material.SetTexture("_ShadowMap", lightManager.shadowMap);

        Matrix4x4 shadowMatrix = lightManager.shadowMatrix;
        material.SetVector("_LightMat0", shadowMatrix.GetRow(0));
        material.SetVector("_LightMat1", shadowMatrix.GetRow(1));
        material.SetVector("_LightMat2", shadowMatrix.GetRow(2));
        material.SetVector("_LightMat3", shadowMatrix.GetRow(3));
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
