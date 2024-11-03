using System.Collections.Generic;

public class EffectUnit
{
    public Effect_Time timeEffect;
    public int ActionTime => timeEffect.current - timeEffect.prev;

    public List<Effect> effects;

    public EffectUnit(PawnEntity agent)
    {
        effects = new List<Effect>();
        timeEffect = new Effect_Time(agent);
        timeEffect.current += agent.Property.actionTime;
    }

    public void Play()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].Play();
        }
        timeEffect.Play();
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
