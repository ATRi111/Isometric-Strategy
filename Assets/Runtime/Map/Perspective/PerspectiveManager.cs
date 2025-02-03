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
    /// ���ĳ��λ���Ƿ���Ҫ͸��
    /// </summary>
    public bool CoverCheck(Vector3Int position)
    {
        Vector3Int p = position + IsometricGridManager.CoverVector;
        while (p.z > 0 && Igm.Contains((Vector2Int)p))
        {
            GridObject gridObject = Igm.GetObject(p);
            if (gridObject == null)
                return true;                //�ڵ��˿�λ�õĵؿ���Ҫ͸��

            if (!gridObject.IsGround)
                return true;                //�ڵ��˷ǵؿ�����ĵؿ���Ҫ͸��
            p -= IsometricGridManager.CoverVector;
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
