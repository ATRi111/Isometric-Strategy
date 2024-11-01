using UnityEngine;

namespace EditorExtend.GridEditor
{
    public static class IsometricGridUtility
    {
        public static Vector3 CellToWorld(Vector3 cellPosition, Vector3 cellSize)
        {
            float x = 0.5f * cellPosition.x * cellSize.x - 0.5f * cellPosition.y * cellSize.x;
            float y = 0.5f * cellPosition.x * cellSize.y + 0.5f * cellPosition.y * cellSize.y + 0.5f * cellPosition.z * cellSize.y * cellSize.z;
            float z = cellPosition.z * cellSize.z;
            return new(x, y, z);
        }

        public static int ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
        public static float ManhattanDistance(Vector3 a, Vector3 b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
        }
        public static float ProjectManhattanDistance(Vector2 a,Vector2 b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
        public static int ProjectManhattanDistance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }
}