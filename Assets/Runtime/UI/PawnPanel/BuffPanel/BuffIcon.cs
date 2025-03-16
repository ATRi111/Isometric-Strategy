using TMPro;

public class BuffIcon : InfoIcon
{
    public Buff buff;
    private TextMeshProUGUI tmp;

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        buff.so.ExtractKeyWords(keyWordList);
    }

    public void SetBuff(Buff buff)
    {
        this.buff = buff;
        tmp.text = buff.displayName;
        info = buff.Description;
    }

    protected override void Awake()
    {
        base.Awake();
        tmp = GetComponent<TextMeshProUGUI>();
    }
}
