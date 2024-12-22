using Services;
using Services.Save;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理玩家可用角色等数据
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager FindInstance()
    {
        return GameObject.Find(nameof(PlayerManager)).GetComponent<PlayerManager>();
    }

    public List<PlayerData> playerList;
    public List<Equipment> unusedEquipmentList;

    public List<int> selectedIndicies;

    public PlayerData Find(string entityName)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].prefab.name == entityName)
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
            data.equipmentList.Add(temp[i]);
        }
    }

    public void ApplyPlayerData(PawnEntity pawn)
    {
        string entityName = pawn.gameObject.name;
        PlayerData data = Find(entityName);
        if (data == null)
            return;

        pawn.EquipmentManager.UnEquipAll();
        for (int i = 0;i < data.equipmentList.Count;i++)
        {
            pawn.EquipmentManager.Equip(data.equipmentList[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Save();
        else if (Input.GetKeyUp(KeyCode.L))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Load();
    }
}