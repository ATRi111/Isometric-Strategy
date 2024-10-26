using System.Collections.Generic;

public class EffectUnit
{
    public TimeEffect timeEffect;
    public int ActionTime => timeEffect.current - timeEffect.prev;

    public List<Effect> effects;

    public EffectUnit(PawnEntity agent)
    {
        timeEffect = new TimeEffect(agent);
    }

    public void Apply()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Apply();
        }
        timeEffect.Apply();
    }

    public void Revoke()
    {
        timeEffect.Revoke();
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].Revoke();
        }
    }
}
