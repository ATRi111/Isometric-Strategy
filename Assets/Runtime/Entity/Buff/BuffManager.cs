using Character;
using MyTool;
using Services;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private PawnEntity pawn;
    private GameManager gameManager;
    [SerializeField]
    private SerializedHashSet<Buff> buffs;

    public void OnTick(int time)
    {
        foreach (Buff buff in buffs)
        {
            buff.Enabled = time < buff.endTime;
            if (buff.Enabled)
                buff.Tick(time);
        }
    }

    public BuffEffect MockAdd(Buff target, int probability)
    {
        switch(target.so.superimposeMode)
        {
            case ESuperimposeMode.Coexist:
                return new AddBuffEffect(pawn, target, this, probability);
            case ESuperimposeMode.Refresh:
                Buff buff = SuprimposeCheck(target);
                if(buff == null)
                    return new AddBuffEffect(pawn, target, this, probability);
                return new LengthenBuffEffect(pawn, buff, target.endTime, this, probability);
            default:
                throw new System.ArgumentException();
        }
    }

    public bool Contains(Buff target)
    {
        return buffs.Contains(target);
    }

    public Buff SuprimposeCheck(Buff target)
    {
        foreach(Buff buff in buffs)
        {
            if(target.SuperimposeCheck(buff))
                return buff;
        }
        return null;
    }

    public void Add(Buff buff)
    {
        buffs.Add(buff);
        buff.Enabled = gameManager.Time < buff.endTime;
    }

    public void Remove(Buff buff)
    {
        buff.Enabled = false;
        buffs.Remove(buff);
    }

    protected override void Awake()
    {
        base.Awake();
        gameManager = ServiceLocator.Get<GameManager>();
        pawn = entity as PawnEntity;
    }
}
