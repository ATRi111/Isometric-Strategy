using MyTool;
using Services;
using Services.Event;
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

    private IEventSystem eventSystem;

    public List<PlayerData> playerList;
    public List<Equipment> unusedEquipmentList;
    public List<PawnEntity> spawnedPlayers;

    [SerializeField]
    private SerializedHashSet<int> selectedIndices;

    private IsometricGridManager igm;
    private SpawnController spawnController;

    public PlayerData Find(string entityName)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if(playerList[i].prefab.name == entityName)
                return playerList[i];
        }
        return null;
    }

    public int FindIndex(string entityName)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].prefab.name == entityName)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 将当前角色状态记录到数据中
    /// </summary>
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

    /// <summary>
    /// 将记录的数据应用到角色上
    /// </summary>
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

    private void AfterLoadScene(int sceneIndex)
    {
        if (sceneIndex <= 2)
            return;

        igm = IsometricGridManager.FindInstance();
        spawnController = igm.GetComponentInChildren<SpawnController>();
        //TODO:修改出场角色
        int count = 0;
        foreach (int index in selectedIndices)
        {
            GameObject prefab = playerList[index].prefab;
            PawnEntity pawn = spawnController.Spawn(prefab, count);
            ApplyPlayerData(pawn);
            spawnedPlayers.Add(pawn);
            count++;
        }
    }

    private void AfterUnLoadScene(int sceneIndex)
    {
        if (sceneIndex <= 2)
            return;

        spawnedPlayers.Clear();
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterUnLoadScene, AfterUnLoadScene);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
        eventSystem.RemoveListener<int>(EEvent.AfterUnLoadScene, AfterUnLoadScene);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Save();
        else if (Input.GetKeyUp(KeyCode.L))
            ServiceLocator.Get<ISaveManager>().GetGroup(1).Load();
    }
}