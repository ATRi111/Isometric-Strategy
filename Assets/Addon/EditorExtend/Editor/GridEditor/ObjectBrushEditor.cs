using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(ObjectBrush),true)]
    public class ObjectBrushEditor : InteractiveEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;

        [AutoProperty]
        public SerializedProperty cellPosition, prefab;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorModeOnly = true;
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (Application.isPlaying)
                return;

            prefab.PropertyField("笔刷");
            EditorGUI.BeginDisabledGroup(true);
            cellPosition.Vector3IntField("网格位置");
            EditorGUI.EndDisabledGroup();
        }

        //必要时调用currentEvent.Use()
        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            HandleKeyInput();

            if (Application.isPlaying)
                return;

            UpdateCellPosition();
            HandleMouseInput();
        }

        protected override void HandleMouseInput()
        {
            UpdateCellPosition();
            base.HandleMouseInput();
        }

        protected override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            switch(button)
            {
                case 0:
                    Brush();
                    break;
                case 1:
                    Erase();
                    break;
            }
        }
        protected override void OnMouseDrag(int button)
        {
            base.OnMouseDrag(button);
            switch (button)
            {
                case 0:
                    Brush();
                    break;
                case 1:
                    Erase();
                    break;
            }
        }

        protected virtual void UpdateCellPosition()
        {
            Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
            cellPosition.vector3IntValue = ObjectBrush.CalculateCellPosition(world);
        }

        protected virtual void Brush()
        {
            currentEvent.Use();
            if (!ObjectBrush.Manager.CanPlaceAt(cellPosition.vector3IntValue))
                return;

            GridObject gridObject = null;
            if (ObjectBrush.prefab != null)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(ObjectBrush.prefab, ObjectBrush.transform) as GameObject;
                gridObject = obj.GetComponent<GridObject>();
                SerializedObject temp = new(gridObject);
                SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                cellPosition.vector3IntValue = ObjectBrush.cellPosition;
                gridObject.Refresh(ObjectBrush.cellPosition);
                temp.ApplyModifiedProperties();
            }
            ObjectBrush.Manager[ObjectBrush.cellPosition] = gridObject;
        }

        protected virtual void Erase()
        {
            ObjectBrush.Manager[ObjectBrush.cellPosition] = null;
            currentEvent.Use();
        }
    }
}