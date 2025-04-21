using UnityEngine;

public class BuffAnimation : AnimationObject
{
    private GameObject buffAnimation, debuffAnimation;

    public override void Initialize(IAnimationSource source)
    {
        BuffEffect effect = source as BuffEffect;
        int flag = effect.buff.so.buffType switch
        {
            EBuffType.Buff => 1,
            EBuffType.Debuff => -1,
            _ =>0
        };
        if (effect is RemoveBuffEffect)
            flag = -flag;

        if (flag > 0)
        {
            buffAnimation.SetActive(true);
            debuffAnimation.SetActive(false);
        }
        else if (flag < 0)
        {
            buffAnimation.SetActive(false);
            debuffAnimation.SetActive(true);
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
