using EditorExtend.GridEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IsometricGridManager igm;

    private SpawnPoint[] points;

    public PawnEntity Spawn(GameObject prefab, int index)
    {
        Vector3Int cellPosition = points[index].CellPosition;
        points[index].gameObject.SetActive(false);

        GridObject gridObject = igm.GetObject(cellPosition);
        if (gridObject != null)
            Destroy(gridObject.gameObject);
        GameObject obj = Instantiate(prefab, transform);
        obj.name = prefab.name;
        PawnEntity pawn = obj.GetComponent<PawnEntity>();
        pawn.MovableGridObject.CellPosition = cellPosition;
        pawn.MovableGridObject.Refresh();
        return pawn;
    }

    public void Destroy(int index)
    {
        Vector3Int cellPosition = points[index].CellPosition;
        GridObject gridObject = igm.GetObject(cellPosition);
        if(gridObject != null)
            Destroy(gridObject.gameObject);
        points[index].gameObject.SetActive(true);
    }


    private void Awake()
    {
        igm = GetComponentInParent<IsometricGridManager>();
        points = GetComponentsInChildren<SpawnPoint>();
    }
}
