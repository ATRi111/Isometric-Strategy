using EditorExtend.GridEditor;
using System;
using UnityEngine;

public class PerspectiveManager : MonoBehaviour
{
    private IsometricGridManager Igm => IsometricGridManager.Instance;

    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    /// <summary>
    /// ���ĳ��λ���Ƿ���Ҫ͸��
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position + IsometricGridManager.CoverVector;
        if (p.z <= 0)
            return false;
        GridObject gridObject = Igm.GetObject(p);
        if (gridObject == null)         //�ڵ��˿�λ�õĵؿ���Ҫ͸��
        {
            for (; p.z >= 0; p += Vector3Int.back)
            {
                gridObject = Igm.GetObjectXY((Vector2Int)p);
                if (gridObject != null)
                    return true;
            }
            return false;
        }
        return !gridObject.IsGround;    //�ڵ��˷ǵؿ�����ĵؿ���Ҫ͸��
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
