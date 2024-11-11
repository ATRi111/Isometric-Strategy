using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector2Int extend;

    public Vector2Int from;
    public Vector2Int to;

    private void OnDrawGizmosSelected()
    {
        List<Vector2Int> list = EDirectionTool.OverlapInt(from, to);
        for (int i = 0; i <= extend.x; i++)
        {
            for(int j = 0; j <= extend.y; j++)
            {
                Vector2Int p = new(i, j);
                Gizmos.color = list.Contains(p) ? Color.red : Color.green;
                Gizmos.DrawCube((Vector3Int)p, 0.9f * Vector3.one);
            }
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(from.AddZ(-1), to.AddZ(-1));
    }
}
