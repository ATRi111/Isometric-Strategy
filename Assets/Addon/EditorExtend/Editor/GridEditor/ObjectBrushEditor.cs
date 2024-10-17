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

        protected bool isErasing;

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
        }

        protected override void MyOnInspectorGUI()
        {
            if (Application.isPlaying)
                return;

            prefab.PropertyField("笔刷");
            isErasing = EditorGUILayout.Toggle("擦除模式", isErasing);
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
            if (Application.isPlaying)
                return;

            if (isEditting)
            {
                UpdateCellPosition();
                switch (currentEvent.type)
                {
                    case EventType.Repaint:
                        Paint();
                        break;
                    case EventType.MouseDown:
                        if (currentEvent.button == 0)
                            OnLeftMouseDown();
                        break;
                    case EventType.MouseDrag:
                        if (currentEvent.button == 0)
                            OnLeftMouseDrag();
                        break;
                    case EventType.MouseUp:
                        if (currentEvent.button == 0)
                            OnLeftMouseUp();
                        break;
                }
            }
        }
        protected virtual void Paint() { }
        protected virtual void OnLeftMouseDown()
        {
            Brush();
        }
        protected virtual void OnLeftMouseDrag() 
        {
            Brush();
        }
        protected virtual void OnLeftMouseUp() { }

        protected virtual void UpdateCellPosition()
        {
            Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
            cellPosition.vector3IntValue = ObjectBrush.CalculateCellPosition(world);
        }

        protected virtual void Brush()
        {
            if (isErasing)
            {
                ObjectBrush.Manager[ObjectBrush.cellPosition] = null;
                return;
            }

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
    }
}