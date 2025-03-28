using MyTool;
using System.Collections.Generic;
using UIExtend;
using UnityEngine;

public class AlphaController : MonoBehaviour
{
    public static void SetAlpha(SpriteRenderer spriteRenderer, float multiplier)
    {
        Color color = spriteRenderer.color;
        spriteRenderer.color = color.SetAlpha(multiplier * color.a);
    }

    public static void SetAlpha(CanvasGroupPlus canvasGroup, float multiplier)
    {
        canvasGroup.Alpha *= multiplier;
    }

    public List<SpriteRenderer> spriteRenderers;
    public List<CanvasGroupPlus> canvasGroups;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }
}
