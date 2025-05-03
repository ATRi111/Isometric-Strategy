using EditorExtend.GridEditor;
using Services;
using Services.ObjectPools;
using UnityEngine;

public class FaceShadowGenerator : MonoBehaviour
{
    private GridObject gridObject;
    private ShadowManager shadowManager;
    private IObjectManager objectManager;

    public string[] prefabNames;
    public Vector3Int[] cellNormals;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
        shadowManager = GetComponentInParent<ShadowManager>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    private void Start()
    {
        GridObjectPerspectiveController controller = GetComponent<GridObjectPerspectiveController>();
        for (int i = 0; i < prefabNames.Length; i++)
        {
            ShadowVertex vertex = new(gridObject.CellPosition, cellNormals[i]);
            if (shadowManager.VisibleCheck(vertex))
            {
                IMyObject obj = objectManager.Activate(prefabNames[i], transform.position, Vector3.zero, transform);
                FaceShadow shadow = obj.Transform.GetComponent<FaceShadow>();
                shadow.Init(shadowManager, vertex);
                SpriteRenderer spriteRenderer = shadow.GetComponent<SpriteRenderer>();
                controller.spriteRenderers.Add(spriteRenderer);
            }
        }
    }

    private void OnDestroy()
    {
        ObjectPoolUtility.RecycleMyObjects(gameObject);
    }
}
