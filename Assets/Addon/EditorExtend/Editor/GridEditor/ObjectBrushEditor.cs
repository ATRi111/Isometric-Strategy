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
            if (ObjectBrush.prefab != null)
            {
                Instantiate(ObjectBrush.prefab,
                    ObjectBrush.Manager.CellToWorld(ObjectBrush.cellPosition),
                    Quaternion.identity,
                    ObjectBrush.transform);
                
            }
            ObjectBrush.Manager[ObjectBrush.cellPosition] = ObjectBrush.prefab; 
        }
        protected virtual void OnLeftMouseDrag() { }
        protected virtual void OnLeftMouseUp() { }

        protected virtual void UpdateCellPosition()
        {
            Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
            cellPosition.vector3IntValue = ObjectBrush.CalculateCellPosition(world);
        }
    }
}