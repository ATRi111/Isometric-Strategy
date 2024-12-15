using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class PerspectiveController : MonoBehaviour
{
    private IsometricGridManager igm;

    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    /// <summary>
    /// 检查某个位置是否需要透视
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position - IsometricGridManager.CoverVector;
        //while (igm.MaxLayerDict.ContainsKey((Vector2Int)p) && p.z >= 0)
        //{
        //    GridObject gridObject = igm.GetObject(p);
        //    if (gridObject != null && gridObject.IsGround)
        //        return true;
        //    p += IsometricGridManager.CoverVector;
        //}
        //for (p = position + Vector3Int.forward; p.z <= igm.MaxLayerDict[(Vector2Int)p]; p += Vector3Int.forward)
        //{
        //    GridObject gridObject = igm.GetObject(p);
        //    if (gridObject != null && gridObject.IsGround)
        //        return true;
        //}
        while (igm.MaxLayerDict.ContainsKey((Vector2Int)p))
        {
            GridObject gridObject = igm.GetObject(p);
            if (gridObject != null && gridObject.IsGround)
                return false;
            p -= IsometricGridManager.CoverVector;
        }
        return true;
    }

    private void Awake()
    {
        igm = GetComponent<IsometricGridManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
