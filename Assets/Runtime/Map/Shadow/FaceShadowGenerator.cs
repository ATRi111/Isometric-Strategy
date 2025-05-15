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

    private Texture2D AOLeft;
    private Texture2D AORight;
    private Texture2D AOUp;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        lightManager = GetComponentInParent<LightManager>();
        material = GetComponent<SpriteRenderer>().material;
        cellPosition = gridObject.CellPosition;
    }

    private void Start()
    {
        if (!lightManager.VisibleCheck(cellPosition))
            return;

        material.SetColor("_ColorUp", lightManager.radiance_up);
        AOUp = lightManager.GenerateAO(cellPosition, upNormal);
        material.SetTexture("_AOUp", AOUp);

        material.SetColor("_ColorLeft", lightManager.radiance_left);
        AOLeft = lightManager.GenerateAO(cellPosition, leftNormal);
        material.SetTexture("_AOLeft", AOLeft);

        material.SetColor("_ColorRight", lightManager.radiance_right);
        AORight = lightManager.GenerateAO(cellPosition, rightNormal);
        material.SetTexture("_AORight", AORight);

        material.SetFloat("_TestCover", 0);
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
