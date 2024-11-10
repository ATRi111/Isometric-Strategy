using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    internal static class GridPhysics
    {
        public static bool BoxOverlap(Vector3 min, Vector3 extend, Vector3 p)
        {
            return p.x >= min.x && p.x < min.x + extend.x
                && p.y >= min.y && p.y < min.y + extend.y
                && p.z >= min.z && p.z < min.z + extend.z;
        }
        public static bool CylinderOverlap(Vector3 bottomCenter, float height, float radius, Vector3 p)
        {
            if (p.z < bottomCenter.z || p.z >= bottomCenter.z + height)
                return false;
            float projSqrDistance = (p - bottomCenter).ResetZ().sqrMagnitude;
            return projSqrDistance < radius * radius;
        }
        public static bool SphereOverlap(Vector3 center, float radius, Vector3 p)
        {
            return (p - center).sqrMagnitude < radius * radius;
        }
    }
}