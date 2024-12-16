using Services;
using Services.Asset;
using Services.Save;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerSaveData : SaveData
{
    private IAssetLoader assetLoader;
    private PlayerManager Manager => obj as PlayerManager;

    public List<PlayerData> playerList;
    public List<string> unusedEquipmentList;

    public override void Initialize(string identifier, Object obj)
    {
        base.Initialize(identifier, obj);
        assetLoader = ServiceLocator.Get<IAssetLoader>();
    }

    public override void Load()
    {
        Manager.playerDict.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            Manager.playerDict.Add(playerList[i].entityName, playerList[i].Clone() as PlayerData);
        }
        Manager.unusedEquipmentList.Clear();
        for (int i = 0; i < unusedEquipmentList.Count;i++)
        {
            Equipment equipment = assetLoader.Load<Equipment>(unusedEquipmentList[i]);
            Manager.unusedEquipmentList.Add(equipment);
        }
    }

    public override void Save()
    {
        playerList.Clear();
        foreach(PlayerData player in Manager.playerDict.Values)
        {
            playerList.Add(player.Clone() as PlayerData);
        }
        unusedEquipmentList.Clear();
        for (int i = 0; i < Manager.unusedEquipmentList.Count; i++)
        {
            unusedEquipmentList.Add(Manager.unusedEquipmentList[i].name);
        }
    }
}
