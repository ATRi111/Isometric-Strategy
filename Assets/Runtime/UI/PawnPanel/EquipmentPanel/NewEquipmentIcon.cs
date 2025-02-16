using MyTool;
using System;

public class NewEquipmentIcon : IconUI
{
    [NonSerialized]
    public Equipment equipment;
    
    public void SetEquipment(Equipment equipment)
    {
        this.equipment = equipment; 
        info = equipment.name.Bold() + "\n" + equipment.Description;
        image.sprite = equipment.icon;
        canvasGroup.Visible = true;
    }

    protected override void ExtractKeyWords()
    {
        base.ExtractKeyWords();
        if (equipment != null)
            equipment.ExtractKeyWords(keyWordList);
    }
}
