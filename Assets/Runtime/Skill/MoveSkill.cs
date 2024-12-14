using EditorExtend.GridEditor;
using UnityEngine;

/// <summary>
/// 移动类技能
/// </summary>
public abstract class MoveSkill : Skill
{
    public static int DistanceBetween(Vector3Int position, Vector3Int target)
    {
        return IsometricGridUtility.ProjectManhattanDistance((Vector2Int)position, (Vector2Int)target);
    }

    public int actionTimePerUnit = 10;
    public int ZOCActionTime = 10;
    public float speedMultiplier = 1f;

    public static Vector3Int[] ZoneOfControl =
    {
        Vector3Int.left,
        Vector3Int.left + Vector3Int.forward,
        Vector3Int.left - Vector3Int.forward,
        Vector3Int.right,
        Vector3Int.right + Vector3Int.forward,
        Vector3Int.right - Vector3Int.forward,
        Vector3Int.up,
        Vector3Int.up + Vector3Int.forward,
        Vector3Int.up - Vector3Int.forward,
        Vector3Int.down,
        Vector3Int.down + Vector3Int.forward,
        Vector3Int.down - Vector3Int.forward,
    };

    protected bool UnderZOC(PawnEntity agent, IsometricGridManager igm, Vector3Int position)
    {
        for (int i = 0; i < ZoneOfControl.Length; i++)
        {
            Vector3Int temp = position + ZoneOfControl[i];
            if (igm.EntityDict.ContainsKey(temp) && agent.FactionCheck(igm.EntityDict[temp]) == -1)
                return true;
        }
        return false;
    }

    public override int MockTime(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target)
    {
        int time = 0;
        if (UnderZOC(agent, igm, position))
            time += ZOCActionTime;
        return time + DistanceBetween(position, target) * actionTimePerUnit + actionTime;
    }
}
