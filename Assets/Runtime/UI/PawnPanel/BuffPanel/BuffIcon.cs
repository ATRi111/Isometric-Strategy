using TMPro;

public class BuffIcon : IconUI
{
    public Buff buff;
    private TextMeshProUGUI tmp;

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
