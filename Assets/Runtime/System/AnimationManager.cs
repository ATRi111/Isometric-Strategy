using Services;
using System;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : Service,IService
{
    [AutoService]
    private GameManager gameManager;

    public override Type RegisterType => GetType();
    public bool ImmediateMode { get; set; }

    private readonly HashSet<AnimationProcess> currenAnimations = new();

    public void Register(AnimationProcess process)
    {
        if(!ImmediateMode)
        {
            currenAnimations.Add(process);
            process.OnPlay?.Invoke();
        }
    }

    public void Unregister(AnimationProcess process)
    {
        if(!ImmediateMode)
            currenAnimations.Remove(process);
        process.AfterComplete?.Invoke();
    }

    public void StartWait()
    {
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        yield return null;
        while(currenAnimations.Count > 0)
        {
            yield return null;
        }
        yield return null;
        gameManager.MoveOn();
    }
}
