using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorExtend.GridEditor;
using AStar;

public class AStarMap : MonoBehaviour
{
    private IsometricGridManager igm;
    private PathFindingProcess process;

    private void Awake()
    {
        igm = IsometricGridManager.FindInstance();
    }


}
