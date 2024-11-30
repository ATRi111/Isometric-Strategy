using EditorExtend.GridEditor;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector3 mid = 0.5f * Vector3.one;
    
    protected IsometricGridManager igm;
    public IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }
    public GridObject result;

    public Vector3Int from;
    public Vector3Int to;
    public Vector3 velocity;

    private void OnDrawGizmosSelected()
    {
        float deltaTime = 0.1f;
        Vector3 f = from + mid;
        Vector3 t = to + mid;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Igm.CellToWorld(f), 0.1f);
        Gizmos.DrawSphere(Igm.CellToWorld(t), 0.1f);
        if (GridPhysics.InitialVelocityOfParabola(f, t, 30 * Mathf.Deg2Rad, out velocity))
        {
            Vector3 s = f;
            Vector3 v = velocity;
            for (float i = 0f; i < 1f; i += deltaTime)
            {
                s += v * deltaTime;
                v += GridPhysics.Gravity * deltaTime * Vector3.back;
                Gizmos.DrawSphere(Igm.CellToWorld(s), 0.05f);
            }
        }
    }
}