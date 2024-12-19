using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    public static Vector3Int CoverVector = new(1, 1, -2);
    private IsometricGridManager igm;

    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    /// <summary>
    /// 检查某个位置是否需要透视
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position - CoverVector;
        while (igm.MaxLayerDict.ContainsKey((Vector2Int)p))
        {
            GridObject gridObject = igm.GetObject(p);
            if (gridObject != null && gridObject.IsGround)
                return false;
            p -= CoverVector;
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
