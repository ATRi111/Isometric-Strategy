using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static int SampleDistance = 20;
    public static Vector3 CellSize = new(1f, 1f, 0.5f);

    private static LightManager instance;
    public static LightManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = GameObject.Find(nameof(LightManager));
                if (obj != null)
                    instance = obj.GetComponent<LightManager>();
            }
            return instance;
        }
    }

    private IsometricGridManager igm;
    private readonly HashSet<Vector3Int> objectCache = new();
    private readonly HashSet<Vector3Int> surfaceCache = new();

    public float projectShadowIntensity = 0.5f;
    public Color lightColor;
    public Vector3 lightDirection;
    public int texelSize;

    private float[,] depths;
    public Texture2D shadowMap;
    public Matrix4x4 lightMatrix;
    public Matrix4x4 shadowMatrix;

    private int xMin, xMax, yMin, yMax, zMin, zMax;

    public bool VisibleCheck(Vector3Int position)
    {
        return !objectCache.Contains(position + Vector3Int.forward)
            || !objectCache.Contains(position + Vector3Int.left)
            || !objectCache.Contains(position + Vector3Int.down);
    }

    private float GetHeight(Vector3Int basePosition, Vector3Int normal)
    {
        int h = 0;
        while (objectCache.Contains(basePosition))
        {
            basePosition += normal;
            h++;
        }
        return h - 1;
    }

    public Vector4 CellToLightSpace(Vector4 cell)
    {
        Vector4 lightSpace = lightMatrix * cell;
        return texelSize * lightSpace;
    }

    public Vector4 CellToShadowMapCoord(Vector4 cell)
    {
        return shadowMatrix * cell;
    }

    public Vector4 ShadowMapCoordToCell(Vector4 coord)
    {
        return shadowMatrix.inverse * coord;
    }

    private void UpdateCoordSystem()
    {
        Vector3 lightZ = lightDirection.normalized;
        Vector3 lightX = new Vector3(-lightDirection.z, 0, lightDirection.x).normalized;  //任选一个与lightDirection垂直的向量
        Vector3 lightY = Vector3.Cross(lightX, lightZ).normalized;
        lightMatrix = new Matrix4x4(lightX, lightY, lightZ, new Vector4(0, 0, 0, 1)).inverse;

        xMin = yMin = zMin = int.MaxValue;
        xMax = yMax = zMax = int.MinValue;
        GroundShadowCaster[] shadows = igm.GetComponentsInChildren<GroundShadowCaster>();
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            objectCache.Add(cellPosition);
        }
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            shadows[i].visible = VisibleCheck(cellPosition);
            if (shadows[i].visible)
            {
                surfaceCache.Add(cellPosition);
                Vector4 cell = new(cellPosition.x + 0.5f, cellPosition.y + 0.5f, cellPosition.z + 0.5f, 1);
                Vector4 lightSpace = CellToLightSpace(cell);
                xMin = Mathf.Min(xMin, Mathf.CeilToInt(lightSpace.x));
                xMax = Mathf.Max(xMax, Mathf.FloorToInt(lightSpace.x));
                yMin = Mathf.Min(yMin, Mathf.CeilToInt(lightSpace.y));
                yMax = Mathf.Max(yMax, Mathf.FloorToInt(lightSpace.y));
                zMin = Mathf.Min(zMin, Mathf.CeilToInt(lightSpace.z));
                zMax = Mathf.Max(zMax, Mathf.FloorToInt(lightSpace.z));
            }
        }
        xMin -= texelSize;
        yMin -= texelSize;
        xMax += texelSize;
        yMax += texelSize;

        shadowMatrix = lightMatrix;
        shadowMatrix = Matrix4x4.Scale(texelSize * Vector3.one) * shadowMatrix;
        shadowMatrix = Matrix4x4.Translate(new Vector3(-xMin, -yMin, -zMin)) * shadowMatrix;
        shadowMatrix = Matrix4x4.Scale(new Vector3(1f / (xMax - xMin), 1f / (yMax - yMin), 1f / (zMax - zMin))) * shadowMatrix;
    }

    private void UpdateShadowOfFace(Vector3 origin, Vector3 xAxis, Vector3 yAxis)
    {
        void WriteDepth(Vector4 coord)
        {
            int x = Mathf.FloorToInt(Mathf.Clamp01(coord.x) * shadowMap.width);
            int y = Mathf.FloorToInt(Mathf.Clamp01(coord.y) * shadowMap.height);
            depths[x, y] = Mathf.Clamp01(Mathf.Min(depths[x, y], coord.z));
        }

        for (int x = 0; x <= texelSize; x++)
        {
            for (int y = 0; y <= texelSize; y++)
            {
                Vector3 cell = origin + x / (float)texelSize * xAxis + y / (float)texelSize * yAxis;
                Vector4 coord = CellToShadowMapCoord(new Vector4(cell.x, cell.y, cell.z, 1));
                WriteDepth(coord);
            }
        }
    }

    private void UpdateShadowOfBlock(Vector3Int cellPosition)
    {
        Vector3 cell = cellPosition;
        UpdateShadowOfFace(cell + Vector3.forward, Vector3.right, Vector3.up);
        UpdateShadowOfFace(cell + Vector3.up, Vector3.down, Vector3.forward);
        UpdateShadowOfFace(cell + Vector3.zero, Vector3.right, Vector3.forward);
    }

    private void GenerateShadowMap()
    {
        shadowMap = new(xMax - xMin, yMax - yMin, TextureFormat.RHalf, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp,
        };
        depths = new float[shadowMap.width, shadowMap.height];
        for (int x = 0; x < shadowMap.width; x++)
        {
            for (int y = 0; y < shadowMap.height; y++)
            {
                depths[x, y] = 1;
            }
        }
        foreach (Vector3Int p in surfaceCache)
        {
            UpdateShadowOfBlock(p);
        }
        for (int x = 0; x < shadowMap.width; x++)
        {
            for (int y = 0; y < shadowMap.height; y++)
            {
                shadowMap.SetPixel(x, y, new Color(depths[x, y], 0, 0, 1));
            }
        }
        shadowMap.Apply();
    }

    private void Awake()
    {
        igm = IsometricGridManager.Instance;
        UpdateCoordSystem();
        GenerateShadowMap();
    }

    private void OnDestroy()
    {
        Destroy(shadowMap);
    }
}
