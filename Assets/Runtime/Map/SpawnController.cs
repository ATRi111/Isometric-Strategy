using Services;
using Services.Asset;
using Services.Event;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IEventSystem eventSystem;
    private IAssetLoader assetLoader;
    private PlayerManager playerManager;

    private void BeforeBattle()
    {
        SpawnPoint[] points = GetComponentsInChildren<SpawnPoint>();
        int i = 0;
        for (; i < playerManager.selectedIndicies.Count; i++)
        {
            int index = playerManager.selectedIndicies[i];
            Vector3Int temp = points[i].CellPosition;
            Destroy(points[i].gameObject);
            Spawn(playerManager.playerList[index].entityName, temp);
        }
        for (; i < points.Length; i++)
        {
            Destroy(points[i].gameObject);
        }
    }

    public void Spawn(string entityName, Vector3Int point)
    {
        GameObject prefab = assetLoader.Load<GameObject>(entityName);
        GameObject obj = Instantiate(prefab, transform);
        obj.name = entityName;
        PawnEntity pawn = obj.GetComponent<PawnEntity>();
        pawn.MovableGridObject.CellPosition = point;
        pawn.MovableGridObject.Refresh();
        playerManager.ApplyPlayerData(pawn);
    }


    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
        assetLoader = ServiceLocator.Get<IAssetLoader>();
        eventSystem = ServiceLocator.Get<IEventSystem>();
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.BeforeBattle, BeforeBattle);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.BeforeBattle, BeforeBattle);
    }
}
