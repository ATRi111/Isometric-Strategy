using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridObject))]
    public class GridObjectEditor : AutoEditor
    {
        public GridObject GridObject => target as GridObject;
        [AutoProperty]
        public SerializedProperty cellPosition;

        private Vector3Int prev;

        protected override void OnEnable()
        {
            base.OnEnable();
            prev = cellPosition.vector3IntValue;
        }

        protected override void MyOnInspectorGUI()
        {
            cellPosition.Vector3IntField("��������");
            if (cellPosition.vector3IntValue != prev)
            {
                prev = cellPosition.vector3IntValue;
                GridObject.Refresh(cellPosition.vector3IntValue);
            }
            if (GUILayout.Button("Z�������"))
            {
                cellPosition.vector3IntValue = GridObject.Align();
            }
        }
    }
}