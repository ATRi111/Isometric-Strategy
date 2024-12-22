using Services;
using Services.Event;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IEventSystem eventSystem;
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
            Spawn(playerManager.playerList[index].prefab, temp);
        }
        for (; i < points.Length; i++)
        {
            Destroy(points[i].gameObject);
        }
    }

    public void Spawn(GameObject prefab, Vector3Int point)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.name = prefab.name;
        PawnEntity pawn = obj.GetComponent<PawnEntity>();
        pawn.MovableGridObject.CellPosition = point;
        pawn.MovableGridObject.Refresh();
        playerManager.ApplyPlayerData(pawn);
    }


    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
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
