using Services;
using Services.Event;
using System;
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

    private IsometricGridManager Igm => IsometricGridManager.Instance;
    private IEventSystem eventSystem;
    private SpawnController spawnController;

    public List<PlayerData> playerDataList;
    public List<Equipment> unusedEquipmentList;
    [NonSerialized]
    public List<PawnEntity> playerList = new();

    public Action AfterSelectChange;

    [SerializeField]
    private List<bool> isSelected;
    [SerializeField]
    private int selectedCount;
    public int SelectedCount => selectedCount;
    public int MaxSelectedCount => spawnController.Count;
    public bool FullySelected => selectedCount >= MaxSelectedCount;

    public PlayerData Find(string entityName)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if(playerDataList[i].prefab.name == entityName)
                return playerDataList[i];
        }
        return null;
    }

    public int FindIndex(string entityName)
    {
        for (int i = 0; i < playerDataList.Count; i++)
        {
            if (playerDataList[i].prefab.name == entityName)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 将当前角色状态记录到数据中
    /// </summary>
    public void UpdatePlayerData(PawnEntity pawn)
    {
        string entityName = pawn.EntityName;
        PlayerData data = Find(entityName);
        if (data == null)
        {
            playerDataList.Add(data);
        }
        List<Equipment> temp = new();
        pawn.EquipmentManager.GetAll(temp);
        for (int i = 0; i < temp.Count; i++)
        {
            data.equipmentList.Add(temp[i]);
        }
    }

    /// <summary>
    /// 将记录的数据应用到角色上
    /// </summary>
    public void ApplyPlayerData(PawnEntity pawn)
    {
        string entityName = pawn.EntityName;
        PlayerData data = Find(entityName);
        if (data == null)
            return;

        pawn.EquipmentManager.UnEquipAll();
        for (int i = 0;i < data.equipmentList.Count;i++)
        {
            pawn.EquipmentManager.Equip(data.equipmentList[i]);
        }
    }

    private void AfterLoadScene(int sceneIndex)
    {
        if (sceneIndex <= 2)
            return;

        spawnController = Igm.GetComponentInChildren<SpawnController>();
    }

    private void BeforeUnLoadScene(int sceneIndex)
    {
        for (int i = 0; i < isSelected.Count; i++)
        {
            Unselect(i);
        }
    }

    /// <summary>
    /// 创建角色（挂载到自身）
    /// </summary>
    public void Generate(PlayerData data)
    {
        GameObject obj = Instantiate(data.prefab, transform);
        obj.name = data.prefab.name;
        playerList.Add(obj.GetComponent<PawnEntity>());
        obj.SetActive(false);
    }
    /// <summary>
    /// 生成角色（挂载在自身的角色移入场景）
    /// </summary>
    public void Spawn(PawnEntity pawn)
    {
        spawnController.Spawn(pawn);
        ApplyPlayerData(pawn);
    }
    /// <summary>
    /// 回收角色（挂载在场景的角色移回自身）
    /// </summary>
    public void Recycle(PawnEntity pawn)
    {
        pawn.gameObject.SetActive(false);
        pawn.transform.SetParent(transform);
    }

    public void Select(int index)
    {
        if (!isSelected[index])
        {
            isSelected[index] = true;
            selectedCount++;
            Spawn(playerList[index]);
            AfterSelectChange?.Invoke();
        }
    }
    public void Unselect(int index)
    {
        if (isSelected[index])
        {
            isSelected[index] = false;
            selectedCount--;
            Recycle(playerList[index]);
            spawnController.Refresh();
            AfterSelectChange?.Invoke();
        }
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        for (int i = 0; i < playerDataList.Count; i++)
        {
            Generate(playerDataList[i]);
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.AddListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.RemoveListener<int>(EEvent.BeforeUnLoadScene, BeforeUnLoadScene);
    }
}