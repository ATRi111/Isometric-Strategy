/// <summary>
/// ���ӷ�ʽ
/// </summary>
public enum ESuperimposeMode
{
    Coexist,
    Refresh
}

[System.Serializable]
public class Buff
{
    public int startTime;
    public int endTime;
    
    public virtual void OnEnable()
    {

    }
    public virtual void OnDisable()
    {

    }
}
