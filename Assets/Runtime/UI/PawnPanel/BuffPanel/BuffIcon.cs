using MyTool;

public class BuffIcon : InfoIcon
{
    public Buff buff;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        buff.so.ExtractKeyWords(KeyWordList);
    }

    public void SetBuff(Buff buff)
    {
        this.buff = buff;
        info = buff.displayName.Bold() + "\n" + buff.Description;
        image.sprite = buff.so.icon;
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
