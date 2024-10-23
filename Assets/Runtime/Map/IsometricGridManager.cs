using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

public class IsometricGridManager : IsometricGridManagerBase
{
    private readonly Dictionary<Vector2Int, Pawn> pawnDict = new();
    public Dictionary<Vector2Int,Pawn> PawnDict => pawnDict;

    public static IsometricGridManager FindInstance()
    {
        return GameObject.Find("Grid").GetComponent<IsometricGridManager>();
    }

    public override void Clear()
    {
        base.Clear();
        pawnDict.Clear();
    }

    public override void AddObject(GridObject gridObject)
    {
        base.AddObject(gridObject);
        if(gridObject is Pawn pawn)
        {
            pawnDict.Add((Vector2Int)gridObject.CellPosition, pawn);
        }
    }
}
