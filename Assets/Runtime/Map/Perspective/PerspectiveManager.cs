using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static PerspectiveManager FindInstance()
    {
        return GameObject.Find(nameof(PerspectiveManager)).GetComponent<PerspectiveManager>();
    }

    private IsometricGridManager Igm => IsometricGridManager.Instance;

    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    /// <summary>
    /// 检查某个位置是否需要透视
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position - IsometricGridManager.CoverVector;
        while (Igm.Contains((Vector2Int)p))
        {
            GridObject gridObject = Igm.GetObject(p);
            if (gridObject != null && gridObject.IsGround)
                return false;
            p -= IsometricGridManager.CoverVector;
        }
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
