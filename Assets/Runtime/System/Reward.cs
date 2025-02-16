using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "��һ�ؽ���", menuName = "�ؿ�����")]
public class Reward : ScriptableObject
{
    public GameObject newPlayer;
    public List<Equipment> equipmentList;

    public void Apply(PlayerManager playerManager)
    {
        if(newPlayer != null)
        {
            playerManager.AddPlayerData(PlayerData.FromPrefab(newPlayer));
        }
        for (int i = 0; i < equipmentList.Count; i++)
        {
            playerManager.unusedEquipmentList.Add(equipmentList[i]);
        }
    }
}