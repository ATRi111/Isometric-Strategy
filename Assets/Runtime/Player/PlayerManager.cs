using Services;
using Services.Asset;
using Services.Event;
using Services.Save;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理玩家可用角色数据
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager FindInstance()
    {
        return GameObject.Find(nameof(PlayerManager)).GetComponent<PlayerManager>();
    }

    private IEventSystem eventSystem;
    private IsometricGridManager igm;
    private IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

    private IAssetLoader assetLoader;
    public List<PlayerData> playerList;
    public List<Equipment> unusedEquipmentList;

    public List<int> selectedIndicies;

    public PlayerData Find(string entityName)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].entityName == entityName)
                return playerList[i];
        }
        return null;
    }

    public void UpdatePlayerData(PawnEntity pawn)
    {
        string entityName = pawn.gameObject.name;
        PlayerData data = Find(entityName);
        if (data == null)
        {
            playerList.Add(data);
        }
        List<Equipment> temp = new();
        pawn.EquipmentManager.GetAll(temp);
        for (int i = 0; i < temp.Count; i++)
        {
            data.equipmentList.Add(temp[i].name);
        }
    }

    public void ApplyPlayerData(PawnEntity pawn)
    {
        string entityName = pawn.gameObject.name;
        PlayerData data = Find(entityName);
        if (data == null)
            return;

        pawn.EquipmentManager.UnEquipAll();
        List<string> temp = data.equipmentList;
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