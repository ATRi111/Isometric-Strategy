using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : Service,IService
{
    [AutoService]
    private GameManager gameManager;

    public override Type RegisterType => GetType();
    public bool ImmediateMode { get; set; }

    private readonly HashSet<AnimationProcess> currenAnimations = new();

    public Action AfterNoAnimation;

    public void Register(AnimationProcess animation)
    {
        if(!ImmediateMode)
        {
            currenAnimations.Add(animation);
            animation.Play();
        }
        else
            Unregister(animation);
    }

    public void Unregister(AnimationProcess animation)
    {
        if(!ImmediateMode)
            currenAnimations.Remove(animation);
        animation.Apply();
        if (currenAnimations.Count == 0)
            StartCoroutine(NoAnimationCheck());
    }

    public IEnumerator NoAnimationCheck()
    {
        yield return new WaitForEndOfFrame();
        if (currenAnimations.Count == 0)
            AfterNoAnimation?.Invoke();
    }
}
