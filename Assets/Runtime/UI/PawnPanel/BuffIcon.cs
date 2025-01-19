public class BuffIcon : IconUI
{
    public Buff buff;

    public void SetBuff(Buff buff)
    {
        this.buff = buff;
        message = buff.Description;
    }
}
