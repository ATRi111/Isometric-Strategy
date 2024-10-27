using System.Collections.Generic;

/// <summary>
/// һ����ɫһ���ж����������ɸ�����
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
        actions.Clear();    //�滮ʱ��ִ��ʱ�ֱ�ģ��
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Mock(agent, igm, effectUnit);
        }
    }
}
