using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "第一关奖励", menuName = "关卡奖励")]
public class Reward : ScriptableObject
{
    public List<GameObject> newPlayerList;
    public List<Equipment> equipmentList;

    public void Apply(PlayerManager playerManager)
    {
        for (int i = 0; i < newPlayerList.Count; i++)
        {
            playerManager.AddPlayerData(PlayerData.FromPrefab(newPlayerList[i]));
        }
        for (int i = 0; i < equipmentList.Count; i++)
        {
            playerManager.unusedEquipmentList.Add(equipmentList[i]);
        }
    }
}