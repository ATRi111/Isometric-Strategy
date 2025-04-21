using UnityEngine;

public class HPChangeAnimation : AnimationObject
{
    private GameObject healAnimation, damageAnimation;

    public override void Initialize(IAnimationSource source)
    {
        HPChangeEffect effect = source as HPChangeEffect;
        int damage = effect.prev - effect.current;
        if (damage > 0)
        {
            damageAnimation.SetActive(true);
            healAnimation.SetActive(false);
        }
        else if (damage < 0)
        {
            damageAnimation.SetActive(false);
            healAnimation.SetActive(true);
        }
        else
        {
            damageAnimation.SetActive(false);
            healAnimation.SetActive(false);
        }
        base.Initialize(source);
    }

    protected override void Awake()
    {
        base.Awake();
        healAnimation = transform.Find("HealAnimation").gameObject;
        damageAnimation = transform.Find("DamageAnimation").gameObject;
    }
}
