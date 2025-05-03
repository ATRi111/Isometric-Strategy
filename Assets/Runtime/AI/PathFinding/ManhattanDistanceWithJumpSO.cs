using AStar;
using UnityEngine;

[CreateAssetMenu(fileName = "�����پ���(������Ծ)", menuName = "AStar/������������ķ���/�����پ���(������Ծ)")]
public class ManhattanDistanceWithJumpSO : CalculateDistanceSO
{
    public float jumpDistanceMultiplier = 1.5f;

    public override float CalculateDistance(Vector2Int from, Vector2Int to)
    {
        if ((from - to).sqrMagnitude == 1)
            return PathFindingUtility.ManhattanDistance(from, to);
        return jumpDistanceMultiplier * PathFindingUtility.ManhattanDistance(from, to);
    }
}