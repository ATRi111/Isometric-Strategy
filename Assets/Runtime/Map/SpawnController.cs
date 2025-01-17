using EditorExtend.GridEditor;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    private IsometricGridManager igm;

    private SpawnPoint[] points;

    public void Spawn(PawnEntity pawn)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].IsEmpty)
            {
                points[i].IsEmpty = false;
                Vector3Int cellPosition = points[i].CellPosition;
                pawn.transform.SetParent(transform);
                pawn.GridObject.Register();
                pawn.MovableGridObject.CellPosition = cellPosition;
                pawn.MovableGridObject.Refresh();
                return;
            }
        }

        Debug.LogWarning("没有空余的生成点");
    }

    public void Recycle(PawnEntity pawn)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (!points[i].IsEmpty)
            {
                Vector3Int cellPosition = points[i].CellPosition;
                GridObject obj = igm.GetObject(cellPosition);
                if (pawn.gameObject == obj)
                {
                    pawn.GridObject.Unregister();
                    points[i].IsEmpty = true;
                    break;
                }
            }
        }
    }

    private void Awake()
    {
        igm = GetComponentInParent<IsometricGridManager>();
        points = GetComponentsInChildren<SpawnPoint>();
    }
}
