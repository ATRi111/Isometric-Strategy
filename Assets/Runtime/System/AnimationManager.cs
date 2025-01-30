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

    private Action ApplyAll;    //Ӧ�ö������������������Ч��������ȷ����˳��Ӧ��
    public Action AfterAnimationComplete;

    /// <summary>
    /// ע�Ტ��latency��󲥷Ŷ���
    /// </summary>
    public void Register(AnimationProcess animation, float latency)
    {
        Debug.Log($"{animation.GetType()} register");
        ApplyAll += animation.Apply;
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
        Debug.Log($"{animation.GetType()} unregister");
        if (!ImmediateMode)
            currenAnimations.Remove(animation);
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
        {
            ApplyAll?.Invoke();
            ApplyAll = null;
            AfterAnimationComplete?.Invoke();
        }
    }
}
