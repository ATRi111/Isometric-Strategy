using System.Collections.Generic;

/// <summary>
/// 一个角色一次行动作出的若干个动作
/// </summary>
public class ActionUnit
{
    protected IsometricGridManager igm;
    public List<PawnAction> actions;
    public EffectUnit effectUnit;
    public PawnEntity agent;

    public ActionUnit(PawnEntity agent, IsometricGridManager igm)
    {
        this.agent = agent;
        this.igm = igm;
    }

    public void Mock()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Mock(agent, igm, effectUnit);
        }
    }
}
