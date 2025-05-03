using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    /// <summary>
    /// 检查某个位置是否需要透视
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position + IsometricGridManager.CoverVector;
        if (p.z <= 0)
            return false;
        GridObject gridObject = Igm.GetObject(p);
        if (gridObject == null)         //遮挡了空位置的地块需要透视
        {
            for (; p.z >= 0; p += Vector3Int.back)
            {
                gridObject = Igm.GetObjectXY((Vector2Int)p);
                if (gridObject != null)
                    return true;
            }
            return false;
        }
        return !gridObject.IsGround;    //遮挡了非地块物体的地块需要透视
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
