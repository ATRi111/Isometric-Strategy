using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector3 mid = 0.5f * Vector3.one;
    
    public IsometricGridManager Igm => IsometricGridManager.Instance;

    public GridObject result;

    public Vector3Int from;
    public Vector3Int to;

    public Vector3 velocity;
    public float time;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float g = GridPhysics.settings.gravity;
        if (GridPhysics.InitialVelocityOfParabola(from + mid, to + mid, 30 * Mathf.Deg2Rad, g, out velocity, out time))
        {
            List<Vector3> vs = new();
            GridPhysics.DiscretizeParabola(from + mid, velocity, g, time, GridPhysics.settings.parabolaPrecision, vs);
            for (int i = 0; i < vs.Count; i++)
            {
                Gizmos.DrawSphere(Igm.CellToWorld(vs[i]), 0.02f);
            }
        }
    }
}