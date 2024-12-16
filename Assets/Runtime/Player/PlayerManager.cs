using Services;
using Services.Asset;
using Services.Save;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理玩家可用角色数据
/// </summary>
public class PlayerManager : MonoBehaviour
{
    private IAssetLoader assetLoader;
    public Dictionary<string, PlayerData> playerDict;
    public List<Equipment> unusedEquipmentList;

    public void UpdatePlayerData(PawnEntity pawn)
    {
        string entityName = pawn.name;
        if (!playerDict.ContainsKey(entityName))
        {
            playerDict.Add(entityName, new PlayerData(entityName));
        }
        List<Equipment> temp = new();
        pawn.EquipmentManager.GetAll(temp);
        for (int i = 0; i < temp.Count; i++)
        {
            playerDict[entityName].equipmentList.Add(temp[i].name);
        }
    }

    public void ApplyPlayerData(PawnEntity pawn)
    {
        string entityName = pawn.name;
        if (!playerDict.ContainsKey(entityName))
            return;

        pawn.EquipmentManager.UnEquipAll();
        List<string> temp = playerDict[entityName].equipmentList;
        for (int i = 0;i < temp.Count;i++)
        {
            Equipment equipment = assetLoader.Load<Equipment>(temp[i]);
            pawn.EquipmentManager.Equip(equipment);
        }
    }

    private void Awake()
    {
        assetLoader = ServiceLocator.Get<IAssetLoader>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Read();
        else if (Input.GetKeyUp(KeyCode.L))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Write();
    }
}