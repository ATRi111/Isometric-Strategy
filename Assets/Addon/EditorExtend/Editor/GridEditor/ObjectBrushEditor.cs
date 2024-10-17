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
        /// �Ƿ��ڱ༭״̬���༭״̬�£��Ż���Ӧ��������¼�
        /// </summary>
        protected bool isEditting;

        protected override void OnEnable()
        {
            base.OnEnable();
            isEditting = false;
        }

        protected override void MyOnInspectorGUI()
        {
            prefab.PropertyField("��ˢ");
            string s = isEditting ? "�����༭" : "��ʼ�༭";
            if (GUILayout.Button(s))
            {
                isEditting = !isEditting;
                focusMode = isEditting ? EFocusMode.Lock : EFocusMode.Default;
                SceneView.RepaintAll();
            }

            if(isEditting)
            {
                EditorGUI.BeginDisabledGroup(true);
                cellPosition.Vector3IntField("����λ��");
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