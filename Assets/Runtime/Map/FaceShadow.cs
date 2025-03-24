using EditorExtend.GridEditor;
using MyTool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaceShadow : MonoBehaviour
{
    public Vector3Int cellNormal;

    private ShadowManager shadowManager;
    private GridObject gridObject;
    private SpriteRenderer myRenderer;
    private ShadowVertex vertex;

    private void Awake()
    {
        shadowManager = GetComponentInParent<ShadowManager>();
        gridObject = GetComponentInParent<GridObject>();
        vertex = new(gridObject.CellPosition, cellNormal);
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(!shadowManager.VisibleCheck(vertex))
        {
            gameObject.SetActive(false);
        }
        else
        {
            float radiance = shadowManager.GetRadiance(vertex);
            myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
        }
    }

#if UNITY_EDITOR
    public void UpdateColor(ShadowManager shadowManager)
    {
        myRenderer = GetComponent<SpriteRenderer>();
        gridObject = GetComponentInParent<GridObject>();
        vertex = new(gridObject.CellPosition, cellNormal);
        float radiance = shadowManager.AmbientOcculasion(vertex);
        myRenderer.color = myRenderer.color.SetAlpha(1f - radiance);
        if (!Application.isPlaying)
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    public void ResetColor()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.color = myRenderer.color.SetAlpha(0f);
        if (!Application.isPlaying)
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
#endif
}
