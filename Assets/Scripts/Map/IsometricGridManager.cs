using EditorExtend.GridEditor;
using UnityEngine;

public class IsometricGridManager : IsometricGridManagerBase
{
    public static IsometricGridManager FindInstance()
    {
        return GameObject.Find("Grid").GetComponent<IsometricGridManager>();
    }
}
