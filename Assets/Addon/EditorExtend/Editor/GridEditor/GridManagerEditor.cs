using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridManager),true)]
    public class GridManagerEditor : AutoEditor
    {
        public GridManager GridManager => target as GridManager;

        [AutoProperty]
        public SerializedProperty centerOffset;

        protected override void MyOnInspectorGUI()
        {
            centerOffset.Vector2Field("����ƫ��");
            if (GUILayout.Button("ȫ��ˢ��"))
            {
                GridObject[] objects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < objects.Length; i++)
                {
                    objects[i].Refresh(objects[i].CellPosition);
                }
            }
        }
    }
}