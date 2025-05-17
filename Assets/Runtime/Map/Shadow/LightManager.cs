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

    public Texture2D shadowMap;

    public Color lightColor;
    public Vector3 lightDirection;
    public int texelSize;

    private int xMin, xMax, yMin, yMax, zMin, zMax;
    private Matrix4x4 lightMatrix;

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
        Vector4 lightSpace = lightMatrix.inverse * cell;
        return texelSize * lightSpace;
    }

    public Vector4 LightSpaceToCell(Vector4 lightSpace)
    {
        lightSpace /= texelSize;
        Vector4 cell = lightMatrix * lightSpace;
        return cell;
    }

    public Vector3 LightSpaceToShadowMapCoord(Vector4 lightSpace)
    {
        float x = Mathf.Clamp01((lightSpace.x - xMin) / (xMax - xMin));
        float y = Mathf.Clamp01((lightSpace.y - yMin) / (yMax - yMin));
        float z = Mathf.Clamp01((lightSpace.z - zMin) / (zMax - zMin));
        return new Vector3(x, y, z);
    }

    public Vector4 ShadowMapCoordToLightSpace(Vector3 coord)
    {
        float x = coord.x * (xMax - xMin) + xMin;
        float y = coord.y * (yMax - yMin) + yMin;
        float z = coord.z * (zMax - zMin) + zMin;
        return new Vector4(x, y, z, 1);
    }

    private void GenerateShadowMap()
    {
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
        shadowMap = new(xMax - xMin, yMax - yMin, TextureFormat.RHalf, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp,
        };
        shadowMap.Apply();
    }

    private void Awake()
    {
        igm = IsometricGridManager.Instance;
        Vector3 lightZ = lightDirection.normalized;
        Vector3 lightX = new Vector3(-lightDirection.z, -lightDirection.z, lightDirection.x + lightDirection.y).normalized;  //任选一个与lightDirection垂直的向量
        Vector3 lightY = Vector3.Cross(lightX, lightZ).normalized;
        lightMatrix = new Matrix4x4(lightX, lightY, lightZ, new Vector4(0, 0, 0, 1));
    }

    private void Start()
    {
        GenerateShadowMap();
    }

    private void OnDestroy()
    {
        Destroy(shadowMap);
    }
}
