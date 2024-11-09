using Character;
using MyTool;
using UnityEngine;

public class BuffManager : CharacterComponentBase
{
    [SerializeField]
    private SerializedHashSet<Buff> buffs;

    public void Register(Buff buff)
    {
        buffs.Add(buff);
        buff.OnEnable();
    }
    public void Unregister(Buff buff)
    {
        buffs.Remove(buff);
        buff.OnDisable();
    }
    public void Refresh(int currentTime)
    {
        foreach (Buff buff in buffs)
        {
            if(currentTime >= buff.endTime)
                Unregister(buff);
        }
    }
}
