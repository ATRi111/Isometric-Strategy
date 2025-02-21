using MyTool;
using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    private ShadowManager shadowManager;
    private SpriteRenderer upRenderer;
    private ShadowVertex[] upSurface;

    public int height = 1;

    private void Awake()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        Vector3 up = height * Vector3Int.forward;
        upSurface = new ShadowVertex[]{
            new(up,Vector3.forward),
            new(Vector3.left + up,Vector3.forward),
            new(Vector3.left + Vector3.right + up,Vector3.forward),
            new(Vector3.right + up,Vector3.forward),
        };
        upRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        float sum = 0f;
        for (int i = 0; i < upSurface.Length; i++)
        {
            sum += 1f - shadowManager.GetVisibility(upSurface[i]);
        }
        upRenderer.color = upRenderer.color.SetAlpha(sum / upSurface.Length);
    }
}
