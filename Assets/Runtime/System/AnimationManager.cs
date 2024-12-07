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

    public Action AfterAnimationComplete;

    /// <summary>
    /// 注册并立即播放动画
    /// </summary>
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
            StartAnimationCheck();
    }

    public void StartAnimationCheck()
    {
        StartCoroutine(AnimationCheck());
    }

    private IEnumerator AnimationCheck()
    {
        yield return new WaitForEndOfFrame();
        if (currenAnimations.Count == 0)
            AfterAnimationComplete?.Invoke();
    }
}
