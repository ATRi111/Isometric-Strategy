using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public static PlayerData FromPrefab(GameObject prefab)
    {
        EquipmentManager equipmentManager = prefab.GetComponentInChildren<EquipmentManager>();
        List<Equipment> equipmentList = new();
        equipmentManager.GetAll(equipmentList);
        return new PlayerData(prefab, equipmentList);
    }

    public GameObject prefab;
    public List<Equipment> equipmentList;

    public PlayerData(GameObject prefab, List<Equipment> equipmentList)
    {
        this.prefab = prefab;
        this.equipmentList = equipmentList;
    }
}