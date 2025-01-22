using EditorExtend.GridEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IsometricGridManager igm;

    private SpawnPoint[] points;

    public int Count => points.Length;

    public void Spawn(PawnEntity pawn)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].IsEmpty)
            {
                points[i].IsEmpty = false;
                pawn.gameObject.SetActive(true);
                Vector3Int cellPosition = points[i].CellPosition;
                pawn.transform.SetParent(transform.parent);
                pawn.MovableGridObject.CellPosition = cellPosition;
                pawn.MovableGridObject.Refresh();
                return;
            }
        }

        Debug.LogWarning("没有空余的生成点");
    }

    //空位自动复原
    public void Refresh()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3Int cellPosition = points[i].CellPosition;
            GridObject obj = igm.GetObject(cellPosition);
            if (obj == null)
            {
                points[i].IsEmpty = true;
                break;
            }
        }
    }

    private void Awake()
    {
        igm = GetComponentInParent<IsometricGridManager>();
        points = GetComponentsInChildren<SpawnPoint>();
    }
}
