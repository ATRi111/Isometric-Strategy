using System;
using UnityEngine.UI;

public class NewPlayerIcon : IconUI
{
    [NonSerialized]
    public PawnEntity player;

    public void SetPlayer(PawnEntity player)
    {
        this.player = player;
        image.sprite = player.icon;
        info = player.EntityName + "\n" + player.pClass.name;
        canvasGroup.Visible = true;
    }

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        keyWordList.Push(player.pClass.name, player.pClass.Description);
    }

    protected override void Awake()
    {
        base.Awake();
        image = GetComponentInParent<Image>();
    }
}
