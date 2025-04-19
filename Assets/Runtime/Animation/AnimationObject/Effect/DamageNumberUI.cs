using MyTool;
using TMPro;

public class DamageNumberUI : AnimationObject
{
    private TextMeshProUGUI tmp;

    public override void Initialize(IAnimationSource source)
    {
        HPChangeEffect effect = source as HPChangeEffect;
        int damage = effect.prev - effect.current;

        FollowHPBar hpBar = effect.victim.GetComponentInChildren<FollowHPBar>();
        tmp.transform.position = hpBar.UseDamageNumberPosition();

        if (damage > 0)
            tmp.text = damage.ToString().ColorText(FontUtility.Red);
        else if (damage < 0)
            tmp.text = $"+{-damage}".ColorText(FontUtility.SpringGreen3);      //�˺�С��0��Ϊ����
        else
            tmp.text = damage.ToString().ColorText(FontUtility.Black);

        base.Initialize(source);
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = string.Empty;
    }
}
