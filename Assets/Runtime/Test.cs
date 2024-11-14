using UnityEngine;

public class Test : MonoBehaviour
{
    public IsometricGridManager igm;
    public Vector2Int extend;

    public Vector2Int from;
    public Vector2Int to;

    private void Awake()
    {
        igm = GetComponent<IsometricGridManager>();
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
