using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricGridManager : GridManager
    {
        public override int CellToSortingOrder(Vector3Int cell)
        {
            return -cell.x - cell.y + cell.z;
        }
    }
}