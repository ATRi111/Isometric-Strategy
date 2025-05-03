using EditorExtend.GridEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包含与地块表面有关的数据
/// </summary>
public class GridSurface : MonoBehaviour
{
    private GridObject gridObject;
    public int difficulty = 1;  //移动难度

    public bool leftLadder;
    public bool rightLadder;

    private void Awake()
    {
        gridObject = GetComponent<GridObject>();
    }

    public void GetLadder(HashSet<Ladder> ret)
    {
        Vector3Int cellPosition = gridObject.CellPosition;
        Ladder ladder;
        if (leftLadder)
        {
            ladder = new()
            {
                cellPosition = cellPosition,
                direction = Vector3Int.left,
            };
            ret.Add(ladder);
        }
        if (rightLadder)
        {
            ladder = new()
            {
                cellPosition = cellPosition,
                direction = Vector3Int.down,
            };
            ret.Add(ladder);
        }
    }
}

public struct Ladder
{
    public Vector3Int cellPosition;
    public Vector3Int direction;

    public override readonly bool Equals(object obj)
    {
        if (obj is Ladder ladder)
        {
            return ladder.cellPosition == cellPosition && ladder.direction == direction;
        }
        return false;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(cellPosition, direction);
    }
}
