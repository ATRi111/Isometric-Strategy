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

    public override void Load()
    {
        manager.playerDataList.Clear();
        for (int i = 0; i < playerList.Count; i++)
        {
            manager.playerDataList.Add(playerList[i].ToPlayerData(assetLoader));
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
        for (int i = 0; i < manager.playerDataList.Count; i++)
        {
            playerList.Add(new PlayerSaveData(manager.playerDataList[i]));
        }
        unusedEquipmentList.Clear();
        for (int i = 0; i < manager.unusedEquipmentList.Count; i++)
        {
            unusedEquipmentList.Add(manager.unusedEquipmentList[i].name);
        }
    }
    public override void Initialize(string identifier, Object obj)
    {
        base.Initialize(identifier, obj);
        assetLoader = ServiceLocator.Get<IAssetLoader>();
        manager = (obj as GameObject).GetComponent<PlayerManager>();
    }
}

[System.Serializable]
public class PlayerSaveData
{
    public string prefabName;
    public readonly List<string> equipmentList;

    public PlayerSaveData()
    {
        equipmentList = new();
    }


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
        List<Equipment> list = new();
        for (int i = 0; i < equipmentList.Count; i++)
        {
            list.Add(assetLoader.Load<Equipment>(equipmentList[i]));
        }
        return new(assetLoader.Load<GameObject>(prefabName), list);
    }
}
