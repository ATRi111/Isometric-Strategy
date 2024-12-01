using AStar;
using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class DebugANodeGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject prefab;

    private PathFindingManager pfm;
    private IsometricGridManager igm;
    private IsometricGridManager Igm
    {
        get
        {
            if (igm == null)
                igm = IsometricGridManager.FindInstance();
            return igm;
        }
    }

    private void Awake()
    {
        pfm = GetComponent<PathFindingManager>();
        pfm.AfterStep += AfterStep;
    }

    private void AfterStep(PathFindingProcess process)
    {
        Clear();
        GameObject obj = new("DebugANode");
        AStarNode[] allnodes = process.GetAllNodes();
        for (int i = 0; i < allnodes.Length; i++)
        {
            PaintNode(allnodes[i] as ANode, obj.transform);
        }
    }

    internal Vector2Int WorldToNode(Vector3 world)
        => new(Mathf.FloorToInt(world.x), Mathf.FloorToInt(world.y));

    internal Vector3 NodeToWorld(ANode node)
    {
        List<GridObject> temp = new();
        Igm.GetObjectsXY(node.Position, temp);
        if (temp.Count == 0)
            return Igm.CellToWorld((Vector3Int)node.Position);

        int i = 0;
        for (; i < temp.Count; i++)
        {
            if (temp[i].GetComponent<MovableGridObject>() == null)
                break;
        }
        if (i >= temp.Count)
            return Igm.CellToWorld((Vector3Int)node.Position);

        Vector3Int cellPosition = temp[i].CellPosition;
        return Igm.CellToWorld(cellPosition).ResetZ();
    }

    public void Clear()
    {
        GameObject obj = GameObject.Find("DebugANode");
        Destroy(obj);
    }

    private void PaintNode(ANode node, Transform parent)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = node.state.ToString();
        obj.transform.SetParent(parent);
        obj.transform.position = NodeToWorld(node);
        obj.GetComponent<DebugANodeUI>().Initialize(node);
    }
#endif
}
