using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class GridBoxCollider : GridCollider
    {
        [SerializeField]
        protected int height;

        public override bool Overlap(Vector3 p)
        {
            return GridUtility.BoxOverlap(CellPosition, Vector3.one.ResetZ(height), p);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            
        }
    }
}