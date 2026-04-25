using Services;
using Services.Asset;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//确保在LightManager之后Awake
[RequireComponent(typeof(LightManager)), DefaultExecutionOrder(-99)]
public class ShadowManager : MonoBehaviour
{
    /// <summary>
    /// 存储所有地块的位置
    /// </summary>
    public readonly HashSet<Vector3Int> grounds = new();

    private LightManager lightManager;
    private IAssetLoader assetLoader;
    private GameManager gameManager;
    private IsometricGridManager igm;

    private RenderTexture shadowMap;
    private Material groundMaterial;
    private Material shadowMapMaterial;
    /// <summary>
    /// 将逻辑坐标转换为ShadowMap的uv的矩阵
    /// </summary>
    public Matrix4x4 shadowMatrix;

    private readonly List<GroundFaceInstance> faces = new();
    private GraphicsBuffer faceBuffer;
    private CommandBuffer cmd;
    private Mesh squareMesh;  //固定使用单位正方形mesh

    private Vector3Int min, max;
    public static int Extend = 1;

    public Vector4 CellToShadowMapCoord(Vector4 cell)
    {
        return shadowMatrix * cell;
    }

    public Vector4 ShadowMapCoordToCell(Vector4 coord)
    {
        return shadowMatrix.inverse * coord;
    }

    public bool VisibleCheck(Vector3Int position)
    {
        return !grounds.Contains(position + Vector3Int.forward)
            || !grounds.Contains(position + Vector3Int.left)
            || !grounds.Contains(position + Vector3Int.down);
    }

    private void GetGroundData()
    {
        faces.Clear();
        grounds.Clear();

        min = int.MaxValue * Vector3Int.one;
        max = int.MinValue * Vector3Int.one;

        GroundShaderController[] shadows = igm.GetComponentsInChildren<GroundShaderController>();
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            grounds.Add(cellPosition);
        }

        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            shadows[i].visible = VisibleCheck(cellPosition);
            if (shadows[i].visible)
            {
                GroundShadowHelper.GenerateShadowFace(grounds, cellPosition, faces);
                min = Vector3Int.Min(min, cellPosition);
                max = Vector3Int.Max(max, cellPosition + Vector3Int.one);
            }
        }
        min -= Extend * Vector3Int.one;
        max += Extend * Vector3Int.one;
    }

    private void UpdateCoordSystem()
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = new Vector3(min.x, min.y, min.z);
        corners[1] = new Vector3(max.x, min.y, min.z);
        corners[2] = new Vector3(min.x, max.y, min.z);
        corners[3] = new Vector3(max.x, max.y, min.z);
        corners[4] = new Vector3(min.x, min.y, max.z);
        corners[5] = new Vector3(max.x, min.y, max.z);
        corners[6] = new Vector3(min.x, max.y, max.z);
        corners[7] = new Vector3(max.x, max.y, max.z);

        Vector3 lightMin = float.MaxValue * Vector3.one;
        Vector3 lightMax = float.MinValue * Vector3.one;

        for (int i = 0; i < 8; i++)
        {
            Vector4 ls = lightManager.CellToLightSpace(corners[i]);

            lightMin = Vector3.Min(lightMin, ls);
            lightMax = Vector3.Max(lightMax, ls);
        }

        float eps = 1e-4f;
        float invX = 1.0f / Mathf.Max(lightMax.x - lightMin.x, eps);
        float invY = 1.0f / Mathf.Max(lightMax.y - lightMin.y, eps);
        float invZ = 1.0f / Mathf.Max(lightMax.z - lightMin.z, eps);

        // 光源空间 -> [0,1]^3
        shadowMatrix =
            Matrix4x4.Scale(new Vector3(invX, invY, invZ)) *
            Matrix4x4.Translate(-lightMin) *
            lightManager.lightMatrix;

        shadowMapMaterial.SetMatrix("_CellToShadowUV", shadowMatrix);
        groundMaterial.SetMatrix("_CellToShadowUV", shadowMatrix);

        GroundShaderController[] shadows = igm.GetComponentsInChildren<GroundShaderController>();
        for (int i = 0; i < shadows.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadows[i].transform.position);
            Vector3 temp = cellPosition + Vector3.forward;
            shadows[i].shadowMapCoord = CellToShadowMapCoord(new Vector4(temp.x, temp.y, temp.z, 1));
        }
    }

    private void GenerateShadowMap()
    {
        if (faces.Count == 0)
            return;

        faceBuffer?.Release();
        faceBuffer = new GraphicsBuffer(
            GraphicsBuffer.Target.Structured,
            faces.Count,
            System.Runtime.InteropServices.Marshal.SizeOf<GroundFaceInstance>()
        );
        faceBuffer.SetData(faces);

        cmd ??= new CommandBuffer { name = "Build Isometric ShadowMap" };
        cmd.Clear();

        cmd.SetRenderTarget(shadowMap);
        cmd.ClearRenderTarget(true, true, Color.white);

        shadowMapMaterial.SetBuffer("_FaceBuffer", faceBuffer);

        cmd.DrawMeshInstancedProcedural(
            squareMesh,
            0,
            shadowMapMaterial,
            0,
            faces.Count,
            null);

        Graphics.ExecuteCommandBuffer(cmd);

        groundMaterial.SetTexture("_ShadowMap", shadowMap);
    }

    private void AfterLightMatrixChange()
    {
        UpdateCoordSystem();
        GenerateShadowMap();
    }

    private void Awake()
    {
        lightManager = GetComponent<LightManager>();
        gameManager = ServiceLocator.Get<GameManager>();
        igm = IsometricGridManager.Instance;
        assetLoader = ServiceLocator.Get<IAssetLoader>();

        groundMaterial = assetLoader.Load<Material>("Material_Ground");
        shadowMapMaterial = assetLoader.Load<Material>("Material_ShadowMap");
        shadowMap = assetLoader.Load<RenderTexture>("ShadowMap");
        squareMesh = GroundShadowHelper.CreateFaceMesh();

        GetGroundData();
        UpdateCoordSystem();
        GenerateShadowMap();

        lightManager.AfterLightMatrixChange += AfterLightMatrixChange;
    }

    private void OnDestroy()
    {
        Destroy(squareMesh);

        faceBuffer?.Release();
        faceBuffer = null;

        cmd?.Release();
        cmd = null;
    }
}
