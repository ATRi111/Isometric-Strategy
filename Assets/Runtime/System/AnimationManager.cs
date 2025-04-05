using MyTimer;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationManager : Service,IService
{
    public override Type RegisterType => GetType();
    public bool ImmediateMode { get; set; }

    public readonly HashSet<AnimationProcess> currenAnimations = new();

    private Action ApplyAll;    //Ӧ�ö������������������Ч��������ȷ����˳��Ӧ��
    public Action AfterAnimationComplete;
    private TimerOnly timer;

    /// <summary>
    /// ע�Ტ��latency��󲥷Ŷ���
    /// </summary>
    public void Register(AnimationProcess animation, float latency)
    {
        ApplyAll += animation.Apply;
        if(!ImmediateMode)
        {
            currenAnimations.Add(animation);
            animation.Play(latency);
        }
        else
            Unregister(animation);
    }

    public void Unregister(AnimationProcess animation)
    {
        if (!ImmediateMode)
            currenAnimations.Remove(animation);
        if (currenAnimations.Count == 0)
            CheckAnimation();
    }

    public void CheckAnimation()
    {
        timer.Restart();
    }

    private void AfterComplete(float _)
    {
        if(currenAnimations.Count == 0)
        {
            ApplyAll?.Invoke();
            ApplyAll = null;
            AfterAnimationComplete?.Invoke();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        timer = new TimerOnly();
        timer.Initialize(0.01f, false);
        timer.AfterComplete += AfterComplete;
    }

#if UNITY_EDITOR
    [SerializeReference]
    public List<AnimationProcess> inspectorAnimations;
    private void Update()
    {
        inspectorAnimations = currenAnimations.ToList();
    }
#endif
}
