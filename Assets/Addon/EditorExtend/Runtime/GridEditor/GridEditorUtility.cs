using UnityEngine;

namespace EditorExtend.GridEditor
{
    public static class GridEditorUtility
    {
        public static int HeightOfSky = 114514;
        public static int HeightOfVoid = -114514;

        internal static Vector3 ResetZ(this Vector3 v, float z = 0f)
            => new(v.x, v.y, z);
        internal static Vector3Int ResetZ(this Vector3Int v, int z = 0)
            => new(v.x, v.y, z);
        internal static Vector3 AddZ(this Vector2 v, float z = 0)
            => new(v.x, v.y, z);
        internal static Vector3Int AddZ(this Vector2Int v, int z = 0)
           => new(v.x, v.y, z);

        internal static Vector3Int Integerized(this Vector3 v)
            => new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));

        public static bool CubeOverlap(Vector3 min, Vector3 extend, Vector3 p)
        {
            return p.x >= min.x && p.x < min.x + extend.x
                && p.y >= min.y && p.y < min.y + extend.y
                && p.z >= min.z && p.z < min.z + extend.z;
        }
        public static bool SphereOverlap(Vector3 center, float radius, Vector3 p)
        {
            return (p - center).sqrMagnitude < radius * radius;
        }
        public static bool CylinderOverlap(Vector3 bottomCenter, float height, float radius, Vector3 p)
        {
            if (p.z < bottomCenter.z || p.z >= bottomCenter.z + height) 
                return false;
            float projSqrDistance = (p - bottomCenter).ResetZ().sqrMagnitude;
            return projSqrDistance < radius * radius;
        }
    }
}