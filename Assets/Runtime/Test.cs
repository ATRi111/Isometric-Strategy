using EditorExtend.GridEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector3 mid = 0.5f * Vector3.one;

    private IsometricGridManager igm;
    public GridObject result;

    public Vector3Int from;
    public Vector3Int to;
    public Vector3 hit;

    private void Awake()
    {
        igm = IsometricGridManager.FindInstance();
    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            result = igm.LineSegmentCast(from, to, out hit);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(igm.CellToWorld(from + mid).ResetZ(), igm.CellToWorld(hit).ResetZ());
            Gizmos.color = Color.green;
            Gizmos.DrawLine(igm.CellToWorld(hit).ResetZ(), igm.CellToWorld(to + mid).ResetZ());
        }
    }
}
