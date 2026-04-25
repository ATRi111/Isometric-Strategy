using Services;
using Services.Asset;
using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class LightManager : MonoBehaviour
{
    public static Vector3 CellSize = new(1f, 1f, 0.5f);

    public Action AfterLightMatrixChange;

    private IAssetLoader assetLoader;
    private Material groundMaterial;

    [SerializeField]
    private Color lightColor;
    public Color LightColor
    {
        get => lightColor;
        set
        {
            lightColor = value;
            Vector4 v = new(lightColor.r, lightColor.g, lightColor.b, 1);
            groundMaterial.SetVector("_LightColor", v);
        }
    }

    [SerializeField]
    private Vector3 lightDirection;
    public Vector3 LightDirection
    {
        get => lightDirection;
        set
        {
            lightDirection = value;
            groundMaterial.SetVector("_LightDirection", lightDirection);
            UpdateCoordSystem();
        }
    }

    /// <summary>
    /// 将逻辑坐标转换为光源空间坐标的矩阵
    /// </summary>
    public Matrix4x4 lightMatrix;

    public Vector4 CellToLightSpace(Vector4 cell)
    {
        return lightMatrix * cell;
    }

    private void UpdateCoordSystem()
    {
        Vector3 lightZ = lightDirection.normalized;
        Vector3 lightX = new Vector3(-lightDirection.z, 0, lightDirection.x).normalized;  //任选一个与lightDirection垂直的向量
        Vector3 lightY = Vector3.Cross(lightX, lightZ).normalized;
        lightMatrix = new Matrix4x4(lightX, lightY, lightZ, new Vector4(0, 0, 0, 1)).inverse;
        AfterLightMatrixChange?.Invoke();
    }

    private void Awake()
    {
        assetLoader = ServiceLocator.Get<IAssetLoader>();
        groundMaterial = assetLoader.Load<Material>("Material_Ground");
        UpdateCoordSystem();
        LightColor = lightColor;
        LightDirection = lightDirection;
    }
}
