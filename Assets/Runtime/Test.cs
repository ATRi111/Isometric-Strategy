using EditorExtend.GridEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private IsometricGridManager igm;
    public GridObject result;

    public Vector3Int from;
    public Vector3Int to;

    private void Awake()
    {
        igm = IsometricGridManager.FindInstance();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 mid = 0.5f * Vector3.one;
        if(Application.isPlaying)
        {
            result = igm.LineSegmentCast(from, to, out Vector3 hit);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(igm.CellToWorld(from + mid).ResetZ(0), igm.CellToWorld(hit + mid).ResetZ(0));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(igm.CellToWorld(hit + mid).ResetZ(0), igm.CellToWorld(to + mid).ResetZ(0));
        }
    }
}
