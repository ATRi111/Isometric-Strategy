using Services;
using Services.Asset;
using Services.Event;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IEventSystem eventSystem;
    private IAssetLoader assetLoader;
    private PlayerManager playerManager;

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

    public List<Vector3Int> points;

    private void BeforeBattle()
    {
        for (int i = 0; i < playerManager.selectedIndicies.Count; i++)
        {
            int index = playerManager.selectedIndicies[i];
            Spawn(playerManager.playerList[index].entityName, points[i]);
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

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(Igm.CellToWorld(points[i] + 0.5f * Vector3.one), 0.2f);
        }
    }
}
