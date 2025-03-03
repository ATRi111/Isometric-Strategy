using MyTool;
using TMPro;

public class DamageNumberUI : AnimationObject
{
    private TextMeshProUGUI tmp;

    public override void Initialize(IAnimationSource source)
    {
        HPChangeEffect effect = source as HPChangeEffect;
        int damage = effect.prev - effect.current;

        if (damage > 0)
            tmp.text = damage.ToString().ColorText(FontUtility.Red);
        else if (damage < 0)
            tmp.text = $"+{-damage}".ColorText(FontUtility.SpringGreen3);      //伤害小于0视为治疗
        else
            tmp.text = damage.ToString().ColorText(FontUtility.Black);

        base.Initialize(source);
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = string.Empty;
    }
}
