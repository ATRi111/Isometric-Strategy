using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    public static int SampleDistance = 20;
    public static Vector3 CellSize = new(1f, 1f, 0.5f);

    public static Vector3Int[] XYDirections =
    {
        Vector3Int.up,
        Vector3Int.left + Vector3Int.up,
        Vector3Int.left,
        Vector3Int.left + Vector3Int.down,
        Vector3Int.down,
        Vector3Int.right + Vector3Int.down,
        Vector3Int.right,
        Vector3Int.right + Vector3Int.up,
    };

    public static Vector3Int[] XZDirections =
    {
        Vector3Int.forward,
        Vector3Int.right + Vector3Int.forward,
        Vector3Int.right,
        //Vector3Int.right + Vector3Int.back,
        //Vector3Int.back,
        //Vector3Int.left + Vector3Int.back,
        Vector3Int.left,
        Vector3Int.left + Vector3Int.forward,
    };

    public static Vector3Int[] YZDirections =
    {
        Vector3Int.forward,
        Vector3Int.up + Vector3Int.forward,
        Vector3Int.up,
        //Vector3Int.up + Vector3Int.back,
        //Vector3Int.back,
        //Vector3Int.down + Vector3Int.back,
        Vector3Int.down,
        Vector3Int.down + Vector3Int.forward,
    };

    private IsometricGridManager igm;
    private readonly HashSet<Vector3Int> shadowCache = new();
    private readonly Dictionary<ShadowVertex, float> visibilityCache = new();
    
    public float GetVisibility(ShadowVertex vertex)
    {
        if (!visibilityCache.ContainsKey(vertex))
            visibilityCache.Add(vertex, AmbientOcculasion(vertex));
        return visibilityCache[vertex];
    }

    public bool VisibleCheck(ShadowVertex vertex)
    {
        if (igm.ObjectDict.TryGetValue(vertex.cellPosition + vertex.cellNormal, out GridObject gridObject))
        {
            if (gridObject.GetComponentInChildren<ShadowObject>() != null)
                return false;
        }
        return true;
    }

    private float AmbientOcculasion(ShadowVertex vertex)
    {
        float visibility = 0f;
        Vector3Int[] directions;
        if (vertex.cellNormal.x != 0)
            directions = YZDirections;
        else if (vertex.cellNormal.y != 0)
            directions = XZDirections;
        else if (vertex.cellNormal.z != 0)
            directions = XYDirections;
        else
            return 0f;

        for (int i = 0; i < directions.Length; i++)
        {
            visibility += AmbientOcculasion(vertex, directions[i]);
        }
        return visibility / directions.Length;
    }

    private float AmbientOcculasion(ShadowVertex vertex, Vector3Int direction)
    {
        float minVisibility = 1f;
        Vector3Int basePosition = vertex.cellPosition;
        float unitDistance = new Vector3(CellSize.x * direction.x, CellSize.y * direction.y, CellSize.z * direction.z).magnitude;
        float unitHeight = vertex.cellNormal.z != 0 ? 0.5f : 1f;
        for (int i = 0; i < SampleDistance; i++)
        {
            basePosition += direction;
            float h = GetHeight(basePosition, vertex.cellNormal) * unitHeight;
            float d = (i + 0.5f) * unitDistance;
            if (h > 0)
            {
                float angle = Mathf.Atan(h / d);
                minVisibility = Mathf.Min(minVisibility, 1f - angle * 2 / Mathf.PI);
            }
        }
        return minVisibility;
    }

    private float GetHeight(Vector3Int basePosition, Vector3Int normal)
    {
        int h = 0;
        while (shadowCache.Contains(basePosition))
        {
            basePosition += normal;
            h++;
        }
        return h - 1;
    }

    private void Awake()
    {
        igm = GetComponent<IsometricGridManager>();
        ShadowObject[] shadowObjects = GetComponentsInChildren<ShadowObject>();
        for (int i = 0; i < shadowObjects.Length; i++)
        {
            Vector3Int cellPosition = igm.WorldToCell(shadowObjects[i].transform.position);
            shadowCache.Add(cellPosition);
        }
    }
}
