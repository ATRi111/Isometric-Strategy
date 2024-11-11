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
        timeEffect.current += agent.actionTime.CurrentValue;
    }

    public void Play(AnimationManager animationManager)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].WillHappen)
                effects[i].Play(animationManager);
        }
        timeEffect.Play(animationManager);
    }
}
