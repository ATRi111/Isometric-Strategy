using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectUnit
{
    public TimeEffect timeEffect;

    [SerializeReference]
    public List<Effect> effects;

    public EffectUnit(PawnEntity agent)
    {
        effects = new List<Effect>();
        timeEffect = new TimeEffect(agent);
        timeEffect.current += agent.actionTime.IntValue;
    }

    public void Play(AnimationManager animationManager, float latency)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i].WillHappen)
                effects[i].Play(animationManager, latency);
        }
        timeEffect.Play(animationManager, latency);
    }
}
