using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(ObjectBrush),true)]
    public class ObjectBrushEditor : AutoEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;

        [AutoProperty]
        public SerializedProperty cellPosition, prefab;

        /// <summary>
        /// 是否处于编辑状态；编辑状态下，才会响应各种鼠标事件
        /// </summary>
        protected bool isEditting;

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
        }

        protected override void MyOnInspectorGUI()
        {
            if (Application.isPlaying)
                return;

            HandleKeyInput();
            prefab.PropertyField("笔刷");
            string s = isEditting ? "结束编辑" : "开始编辑";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                focusMode = isEditting ? EFocusMode.Lock : EFocusMode.Default;
                SceneView.RepaintAll();
            }

            if(isEditting)
            {
                EditorGUI.BeginDisabledGroup(true);
                cellPosition.Vector3IntField("网格位置");
                EditorGUI.EndDisabledGroup();
            }
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            HandleKeyInput();

            if (Application.isPlaying)
                return;

            if (isEditting)
            {
                UpdateCellPosition();
                HandleMouseInput();
            }
        }

        protected virtual void HandleMouseInput()
        {
            UpdateCellPosition();
            switch (currentEvent.type)
            {
                case EventType.Repaint:
                    Paint();
                    break;
                case EventType.MouseDown:
                    OnMouseDown(currentEvent.button);
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag(currentEvent.button);
                    break;
                case EventType.MouseUp:
                    OnLeftMouseUp(currentEvent.button);
                    break;
            }
        }

        protected virtual void HandleKeyInput()
        {
            if (currentEvent == null)
                return;
            switch (currentEvent.type)
            {
                case EventType.KeyDown:
                    OnKeyDown(currentEvent.keyCode);
                    break;
                case EventType.KeyUp:
                    OnKeyUp(currentEvent.keyCode);
                    break;
            }  
        }

        protected virtual void Paint() { }
        protected virtual void OnMouseDown(int button)
        {
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
        protected virtual void OnMouseDrag(int button)
        {
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
        protected virtual void OnLeftMouseUp(int Button) { }
        protected virtual void OnKeyUp(KeyCode keyCode) { }
        protected virtual void OnKeyDown(KeyCode keyCode) { }

        protected virtual void UpdateCellPosition()
        {
            Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
            cellPosition.vector3IntValue = ObjectBrush.CalculateCellPosition(world);
        }

        protected virtual void Brush()
        {
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
            currentEvent.Use();
        }

        protected virtual void Erase()
        {
            ObjectBrush.Manager[ObjectBrush.cellPosition] = null;
            currentEvent.Use();
        }
    }
}