using Character;
using MyTool;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    private PawnEntity pawn;
    private GameManager gameManager;

    [SerializeField]
    private SerializedHashSet<Buff> buffs;

    private readonly Dictionary<string, int> resistanceSearcher = new();
    [SerializeField]
    private List<BuffResistance> buffResistances;

    /// <summary>
    /// 获取当前生效的所有状态
    /// </summary>
    public void GetAllEnabled(List<Buff> ret)
    {
        ret.Clear();
        foreach (Buff buff in buffs)
        {
            if(buff.Enabled)
                ret.Add(buff);
        }
    }

    /// <summary>
    /// 模拟施加状态（新增或刷新）
    /// </summary>
    public BuffEffect MockAdd(BuffSO so, int probability)
        => MockAdd(so, gameManager.Time, probability);

    public BuffEffect MockAdd(BuffSO so, int startTime, int probability)
    {
        Buff buff = new(pawn, so, startTime);
        if (resistanceSearcher.ContainsKey(so.name))
            probability *= (Effect.MaxProbability - resistanceSearcher[so.name]) / Effect.MaxProbability;
        probability = Mathf.Clamp(probability, 0, 100);

        switch (so.superimposeMode)
        {
            case ESuperimposeMode.Coexist:
                return new AddBuffEffect(pawn, buff, this, probability);
            case ESuperimposeMode.Refresh:
                Buff existed = SuprimposeCheck(buff);
                if (existed == null)
                    return new AddBuffEffect(pawn, buff, this, probability);
                return new LengthenBuffEffect(pawn, existed, existed.endTime, this, probability);
            default:
                throw new System.ArgumentException();
        }
    }

    public bool Contains(Buff target)
    {
        return buffs.Contains(target);
    }

    public Buff FindEnabled(BuffSO so)
    {
        foreach(Buff buff in buffs)
        {
            if(buff.so == so && buff.Enabled)
                return buff;
        }
        return null;
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

    public void OnTick(int time)
    {
        foreach (Buff buff in buffs)
        {
            buff.Enabled = time < buff.endTime;
            if (buff.Enabled)
                buff.Tick(time);
        }
    }

    public void Clear()
    {
        foreach (Buff buff in buffs)
        {
            buff.Enabled = false;   
        }
        buffs.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        gameManager = ServiceLocator.Get<GameManager>();
        pawn = entity as PawnEntity;
        for (int i = 0; i < buffResistances.Count; i++)
        {
            resistanceSearcher.Add(buffResistances[i].so.name, buffResistances[i].value);
        }
    }
}
