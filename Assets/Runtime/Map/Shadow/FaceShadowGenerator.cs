using EditorExtend.GridEditor;
using Services.ObjectPools;
using UnityEngine;

public class FaceShadowGenerator : MonoBehaviour
{
    private GridObject gridObject;
    private LightManager lightManager;
    private Material material;
    private Vector3Int cellPosition;
    public Vector3Int upNormal, leftNormal, rightNormal;

    private void GenerateUp()
    {
        ShadowVertex vertex = new(gridObject.CellPosition, upNormal);
        Color color = lightManager.GetRadiance(vertex);
        material.SetColor("_CoverUp", color);
        material.SetFloat("_CoverUpLeft", lightManager.Sample(cellPosition + Vector3Int.forward + Vector3Int.left));
        material.SetFloat("_CoverUpRight", lightManager.Sample(cellPosition + Vector3Int.forward + Vector3Int.right));
        material.SetFloat("_CoverUpUp", lightManager.Sample(cellPosition + Vector3Int.forward + Vector3Int.up));
        material.SetFloat("_CoverUpDown", lightManager.Sample(cellPosition + Vector3Int.forward + Vector3Int.down));
    }

    private void GenerateLeft()
    {
        ShadowVertex vertex = new(gridObject.CellPosition, leftNormal);
        Color color = lightManager.GetRadiance(vertex);
        material.SetColor("_ColorLeft", color);

        material.SetFloat("_CoverLeftLeft", lightManager.Sample(cellPosition + Vector3Int.left + Vector3Int.up));
        material.SetFloat("_CoverLeftRight", lightManager.Sample(cellPosition + Vector3Int.left + Vector3Int.down));
        material.SetFloat("_CoverLeftUp", lightManager.Sample(cellPosition + Vector3Int.left + Vector3Int.forward));
        material.SetFloat("_CoverLeftDown", lightManager.Sample(cellPosition + Vector3Int.left + Vector3Int.back));
    }

    private void GenerateRight()
    {
        ShadowVertex vertex = new(gridObject.CellPosition, rightNormal);
        Color color = lightManager.GetRadiance(vertex);
        material.SetColor("_ColorRight", color);

        material.SetFloat("_CoverRightLeft", lightManager.Sample(cellPosition + Vector3Int.down + Vector3Int.left));
        material.SetFloat("_CoverRightRight", lightManager.Sample(cellPosition + Vector3Int.down + Vector3Int.right));
        material.SetFloat("_CoverRightUp", lightManager.Sample(cellPosition + Vector3Int.down + Vector3Int.forward));
        material.SetFloat("_CoverRightDown", lightManager.Sample(cellPosition + Vector3Int.down + Vector3Int.back));
    }

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        lightManager = GetComponentInParent<LightManager>();
        material = GetComponent<SpriteRenderer>().material;
        cellPosition = gridObject.CellPosition;
    }

    private void Start()
    {
        GenerateUp();
        GenerateLeft();
        GenerateRight();
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
