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

    public float projectShadowIntensity = 0.5f;
    public Color lightColor;
    public Vector3 lightDirection;
    public int texelSize;
    public float sampleSpacing;

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

    public float RayCast(Vector4 coord)
    {
        Vector4 cell = ShadowMapCoordToCell(coord);
        Vector4 delta = Mathf.Max(sampleSpacing, 0.01f) * lightDirection.normalized;
        int sampleTimes = Mathf.FloorToInt(1 / CellToShadowMapCoord(delta).z);
        for (int i = 0; i < sampleTimes; i++, cell += delta)
        {
            Vector3Int cellPosition = new(Mathf.FloorToInt(cell.x), Mathf.FloorToInt(cell.y), Mathf.FloorToInt(cell.z));
            coord = CellToShadowMapCoord(cell);
            if (objectCache.Contains(cellPosition))
                return Mathf.Clamp01(coord.z);
        }
        return 1f;
    }

    private void UpdateCoordSystem()
    {
        Vector3 lightZ = lightDirection.normalized;
        Vector3 lightX = new Vector3(-lightDirection.z, -lightDirection.z, lightDirection.x + lightDirection.y).normalized;  //任选一个与lightDirection垂直的向量
        Vector3 lightY = Vector3.Cross(lightX, lightZ).normalized;
        lightMatrix = new Matrix4x4(lightX, lightY, lightZ, new Vector4(0, 0, 0, 1)).inverse;

        xMin = yMin = zMin = int.MaxValue;
        xMax = yMax = zMax = int.MinValue;
        GroundShadowCaster[] shadows = igm.GetComponentsInChildren<GroundShadowCaster>();
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            objectCache.Add(cellPosition);

            Vector4 cell = new(cellPosition.x + 0.5f, cellPosition.y + 0.5f, cellPosition.z + 0.5f, 1);
            Vector4 lightSpace = CellToLightSpace(cell);
            xMin = Mathf.Min(xMin, Mathf.CeilToInt(lightSpace.x));
            xMax = Mathf.Max(xMax, Mathf.FloorToInt(lightSpace.x));
            yMin = Mathf.Min(yMin, Mathf.CeilToInt(lightSpace.y));
            yMax = Mathf.Max(yMax, Mathf.FloorToInt(lightSpace.y));
            zMin = Mathf.Min(zMin, Mathf.CeilToInt(lightSpace.z));
            zMax = Mathf.Max(zMax, Mathf.FloorToInt(lightSpace.z));
        }
        xMin--;
        yMin--;
        zMin--;
        xMax++;
        yMax++;
        zMax++;

        shadowMatrix = lightMatrix;
        shadowMatrix = Matrix4x4.Scale(texelSize * Vector3.one) * shadowMatrix;
        shadowMatrix = Matrix4x4.Translate(new Vector3(-xMin, -yMin, -zMin)) * shadowMatrix;
        shadowMatrix = Matrix4x4.Scale(new Vector3(1f / (xMax - xMin), 1f / (yMax - yMin), 1f / (zMax - zMin))) * shadowMatrix;
    }

    private void GenerateShadowMap()
    {
        shadowMap = new(xMax - xMin, yMax - yMin, TextureFormat.RHalf, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
        };
        for (int y = 0; y < shadowMap.height; y++)
        {
            for (int x = 0; x < shadowMap.width; x++)
            {
                Vector4 coord = new((x + 0.5f) / shadowMap.width, (y + 0.5f) / shadowMap.height, 0, 1);
                float depth = RayCast(coord);
                shadowMap.SetPixel(x, y, new Color(depth, 0, 0, 1));
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
