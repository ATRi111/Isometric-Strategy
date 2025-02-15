using MyTool;
using TMPro;

public class DamageNumberUI : AnimationObject
{
    private TextMeshProUGUI tmp;

    public override void Activate(IAnimationSource source)
    {
        HPChangeEffect effect = source as HPChangeEffect;
        int damage = effect.prev - effect.current;
        if (damage == 0)
        {
            myObject.Recycle();
            return;
        }

        if (damage > 0)
            tmp.text = damage.ToString().ColorText(FontUtility.Red);
        else
            tmp.text = $"+{-damage}".ColorText(FontUtility.SpringGreen3);      //�˺�С��0��Ϊ����
        StartCoroutine(DelayRecycle(lifeSpan));
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = string.Empty;
    }
}
