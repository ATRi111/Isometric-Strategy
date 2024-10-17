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
            RefreshObjects();
            centerOffset.Vector2Field("����ƫ��");
            if (GUILayout.Button("ȫ��ˢ��"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    gridObjects[i].Refresh();
                }
            }
            if (GUILayout.Button("ȫ��Z�������"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    SerializedObject temp = new(gridObjects[i]);
                    SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                    cellPosition.vector3IntValue = gridObjects[i].AlignXY();
                    temp.ApplyModifiedProperties();
                }
            }
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            RefreshObjects();
        }

        protected void RefreshObjects()
        {
            if(!Application.isPlaying)
            {
                GridObject[] objects = GridManager.GetComponentsInChildren<GridObject>();
                if (GridManager.ObjectDict.Count != objects.Length)
                    GridManager.AddAllObjects();
            }
        }
    }
}