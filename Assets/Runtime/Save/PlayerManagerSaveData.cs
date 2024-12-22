using Services;
using Services.Asset;
using Services.Save;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManagerSaveData : SaveData
{
    private IAssetLoader assetLoader;
    private PlayerManager manager;

    public readonly List<PlayerSaveData> playerList = new();
    public readonly List<string> unusedEquipmentList = new();

    public override void Initialize(string identifier, Object obj)
    {
        base.Initialize(identifier, obj);
        assetLoader = ServiceLocator.Get<IAssetLoader>();
        manager = (obj as GameObject).GetComponent<PlayerManager>();
    }

    public override void Load()
    {
        manager.playerList.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            manager.playerList.Add(playerList[i].ToPlayerData(assetLoader));
        }
        manager.unusedEquipmentList.Clear();
        for (int i = 0; i < unusedEquipmentList.Count;i++)
        {
            Equipment equipment = assetLoader.Load<Equipment>(unusedEquipmentList[i]);
            manager.unusedEquipmentList.Add(equipment);
        }
    }

    public override void Save()
    {
        playerList.Clear(); 
        for (int i = 0; i < manager.playerList.Count; i++)
        {
            playerList.Add(new PlayerSaveData(manager.playerList[i]));
        }
        unusedEquipmentList.Clear();
        for (int i = 0; i < manager.unusedEquipmentList.Count; i++)
        {
            unusedEquipmentList.Add(manager.unusedEquipmentList[i].name);
        }
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public string prefabName;
    public readonly List<string> equipmentList;

    public PlayerSaveData(PlayerData playerData)
    {
        prefabName = playerData.prefab.name;
        equipmentList = new();
        for (int i = 0; i < playerData.equipmentList.Count; i++)
        {
            equipmentList.Add(playerData.equipmentList[i].name);
        }
    }

    public PlayerData ToPlayerData(IAssetLoader assetLoader)
    {
        PlayerData ret = new()
        {
            prefab = assetLoader.Load<GameObject>(prefabName),
            equipmentList = new()
        };
        for (int i = 0;i < equipmentList.Count;i++)
        {
            ret.equipmentList.Add(assetLoader.Load<Equipment>(equipmentList[i]));
        }
        return ret;
    }
}
