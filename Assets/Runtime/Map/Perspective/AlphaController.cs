using UnityEngine;

public class AlphaController : MonoBehaviour
{
    public static void SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        Color color = spriteRenderer.color;
        color = new Color(color.r, color.g, color.b, alpha);
        spriteRenderer.color = color;
    }

    [SerializeField]
    protected SpriteRenderer[] spriteRenderers;
    protected float[] alphas;

    protected virtual void Awake()
    {
        alphas = new float[spriteRenderers.Length];
        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            alphas[i] = spriteRenderers[i].color.a;
        }
    }

    protected virtual void Start()
    {

    }
}
