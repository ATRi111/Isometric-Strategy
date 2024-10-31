using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(ObjectBrush),true)]
    public class ObjectBrushEditor : InteractiveEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;

        private string[] displayOptions;
        [AutoProperty]
        public SerializedProperty cellPosition, prefab, mountIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorModeOnly = true;
            ObjectBrush.MountPoints = null;
            int n = ObjectBrush.MountPoints.Count;
            displayOptions = new string[n];
            for (int i = 0; i < n; i++)
            {
                displayOptions[i] = ObjectBrush.MountPoints[i].gameObject.name;
            }
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (Application.isPlaying)
                return;

            prefab.PropertyField("��ˢ");
            mountIndex.intValue = EditorGUILayout.Popup("���ص�", mountIndex.intValue, displayOptions);
            EditorGUI.BeginDisabledGroup(true);
            cellPosition.Vector3IntField("����λ��");
            EditorGUI.EndDisabledGroup();
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            if (Application.isPlaying)
                return;

            HandleKeyInput();
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

            if (ObjectBrush.prefab != null)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(ObjectBrush.prefab, ObjectBrush.MountPoint) as GameObject;
                GridObject gridObject = obj.GetComponent<GridObject>();
                SerializedObject temp = new(gridObject);
                SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                cellPosition.vector3IntValue = ObjectBrush.cellPosition;
                gridObject.CellPosition = ObjectBrush.cellPosition;
                temp.ApplyModifiedProperties();
                //ObjectBrush.Manager.AddObject(gridObject);   //Editorģʽ��GridManager���Զ�ˢ���Ի�ȡ�µ�GridObject
            }
            else
                Erase();
        }

        protected virtual void Erase()
        {
            if(ObjectBrush.Manager.ObjectDict.ContainsKey(cellPosition.vector3IntValue))
            {
                GridObject gridObject = ObjectBrush.Manager.RemoveObject(ObjectBrush.cellPosition);
                ExternalTool.Destroy(gridObject.gameObject);
            }
            currentEvent.Use();
        }
    }
}