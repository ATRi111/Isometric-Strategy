using System.Collections.Generic;

public class EffectUnit
{
    public TimeEffect timeEffect;
    public int ActionTime => timeEffect.current - timeEffect.prev;

    public List<Effect> effects;

    public EffectUnit(PawnEntity agent)
    {
        effects = new List<Effect>();
        timeEffect = new TimeEffect(agent);
        timeEffect.current += agent.actionTime.IntValue;
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
