using MyTimer;
using MyTool;
using UnityEngine;

public class DieAnimation : AnimationObject
{
    private LinearTransformation linear;
    private const float fadeTime = 0.2f;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        linear = new LinearTransformation();
        linear.OnTick += OnTick;
        linear.AfterComplete += AfterComplete;
    }

    public override void Initialize(IAnimationSource source)
    {
        base.Initialize(source);
        DisableEntityEffect effect = source as DisableEntityEffect;
        Entity victim = effect.victim;
        spriteRenderer = victim.GetComponentInChildren<SpriteRenderer>();
        linear.Initialize(1f, 0f, fadeTime, true);
    }

    private void OnTick(float value)
    {
        spriteRenderer.color = spriteRenderer.color.SetAlpha(value);
    }

    private void AfterComplete(float _)
    {
        spriteRenderer.color = spriteRenderer.color.SetAlpha(0f);
    }

    private void OnDisable()
    {
        linear.Paused = true;
    }
}
