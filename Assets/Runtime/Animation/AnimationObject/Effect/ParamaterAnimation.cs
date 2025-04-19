using UnityEngine;

public class ParamaterAnimation : AnimationObject
{
    private GameObject buffAnimation, debuffAnimation;

    public override void Initialize(IAnimationSource source)
    {
        ModifyParameterEffect effect = source as ModifyParameterEffect;
        int flag = effect.value - effect.value_prev;

        if (flag > 0)
        {
            buffAnimation.SetActive(true);
            debuffAnimation.SetActive(false);
        }
        else
        {
            buffAnimation.SetActive(false);
            debuffAnimation.SetActive(false);
        }
        base.Initialize(source);
    }

    protected override void Awake()
    {
        base.Awake();
        buffAnimation = transform.Find("BuffAnimation").gameObject;
        debuffAnimation = transform.Find("DebuffAnimation").gameObject;
    }
}
