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
    private Texture3D texture;

    private Vector3Int IndexToOffset(int index)
    {
        int z = index / 9;
        index %= 9;
        int y = index / 3;
        index %= 3;
        int x = index;
        return new Vector3Int(x, y, z) - Vector3Int.one;
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
        ShadowVertex vertex;
        Color color;
        vertex = new(gridObject.CellPosition, upNormal);
        color = lightManager.GetRadiance(vertex);
        material.SetColor("_ColorUp", color);

        vertex = new(gridObject.CellPosition, leftNormal);
        color = lightManager.GetRadiance(vertex);
        material.SetColor("_ColorLeft", color);

        vertex = new(gridObject.CellPosition, rightNormal);
        color = lightManager.GetRadiance(vertex);
        material.SetColor("_ColorRight", color);

        texture = new Texture3D(3, 3, 3, TextureFormat.R8, false)
        {
            filterMode = FilterMode.Point
        };
        for (int z = 0; z < 3; z++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector3Int p = cellPosition + new Vector3Int(x, y, z) - Vector3Int.one;
                    float r = lightManager.Sample(p);
                    texture.SetPixel(x, y, z, new Color(r, 0, 0, 1));
                }
            }
        }
        texture.Apply();
        material.SetTexture("_Cover", texture);
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
