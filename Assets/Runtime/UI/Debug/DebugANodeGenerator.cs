using AStar;
using UnityEngine;

public class DebugANodeGenerator : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject prefab;

    private PathFindingManager pfm;
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    private void Awake()
    {
        pfm = GetComponent<PathFindingManager>();
        pfm.AfterStep += AfterStep;
    }

    private void AfterStep(PathFindingProcess process)
    {
        Clear();
        GameObject obj = new("DebugANode");
        Node[] allNodes = process.GetAllNodes();
        for (int i = 0; i < allNodes.Length; i++)
        {
            PaintNode(allNodes[i] as ANode, obj.transform);
        }
    }

    internal Vector3 NodeToWorld(ANode node)
    {
        return Igm.CellToWorld(node.CellPosition);
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
