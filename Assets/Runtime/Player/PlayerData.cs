using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData : ICloneable
{
    public string entityName;
    public List<string> equipmentList;

    public PlayerData(string entityName)
    {
        this.entityName = entityName;
        equipmentList = new();
    }

    public object Clone()
    {
        PlayerData data = new(entityName);
        data.equipmentList.AddRange(equipmentList);
        return data;
    }
}
