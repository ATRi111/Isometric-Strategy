using System;

public class NewPlayerIcon : InfoIcon
{
    [NonSerialized]
    public PawnEntity player;

    public void SetPlayer(PawnEntity player)
    {
        this.player = player;
        image.sprite = player.icon;
        info = player.EntityName + " " + player.pClass.name;
        canvasGroup.Visible = true;
    }

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        KeyWordList.Push(player.EntityName, player.Description);
        KeyWordList.Push(player.pClass.name, player.pClass.Description);
    }
}
