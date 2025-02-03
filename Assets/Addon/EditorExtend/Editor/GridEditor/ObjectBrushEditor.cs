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
        public SerializedProperty prefab, mountIndex, overrideMode;
        private string prefabName;

        protected Vector3Int lockedPosition;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorModeOnly = true;
            ObjectBrush.MountPoints = null;
            int n = ObjectBrush.MountPoints.Count;
            displayOptions = new string[n];
            prefabName = string.Empty;
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

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Vector3IntField("当前坐标", ObjectBrush.cellPosition);
            EditorGUI.EndDisabledGroup();
            prefab.PropertyField("笔刷");
            if (prefab.objectReferenceValue != null && prefabName != prefab.objectReferenceValue.name)
            {
                prefabName = prefab.objectReferenceValue.name;
                UpdateMountPoint(prefabName);
            }
            mountIndex.intValue = EditorGUILayout.Popup("挂载点", mountIndex.intValue, displayOptions);
            overrideMode.BoolField("强制覆盖模式");
        }

        protected void UpdateMountPoint(string prefabName)
        {
            Transform[] transforms = ObjectBrush.GetComponentsInChildren<Transform>();
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].gameObject.name == prefabName)
                {
                    for (int j = 0; j < displayOptions.Length; j++)
                    {
                        if (transforms[i].parent.name == displayOptions[j])
                        {
                            mountIndex.intValue = j;
                            return;
                        }
                    }
                }
            }
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
            ObjectBrush.cellPosition = ObjectBrush.CalculateCellPosition(world, lockedPosition);
            Repaint();
            SceneView.RepaintAll();
        }

        protected virtual bool TryBrushAt(Vector3Int position)
        {
            if (ObjectBrush.prefab == null)
                return false;

            if (!ObjectBrush.Manager.CanPlaceAt(position))
            {
                if(overrideMode.boolValue)
                    TryEraseAt(position);
                else
                    return false;
            }

            GameObject obj = PrefabUtility.InstantiatePrefab(ObjectBrush.prefab, ObjectBrush.MountPoint) as GameObject;
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            GridObject gridObject = obj.GetComponent<GridObject>();

            SerializedObject temp = new(gridObject);
            SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
            cellPosition.vector3IntValue = position;
            gridObject.CellPosition = position;
            temp.ApplyModifiedProperties();

            return true;
        }

        protected virtual void Brush()
        {
            currentEvent.Use();
            TryBrushAt(ObjectBrush.cellPosition);
        }

        protected virtual bool TryEraseAt(Vector3Int position)
        {
            if (ObjectBrush.Manager.ObjectDict.ContainsKey(position))
            {
                GridObject gridObject = ObjectBrush.Manager.TryRemoveObject(position);
                if(gridObject != null)
                {
                    ExternalTool.Destroy(gridObject.gameObject);
                    return true;
                }
            }
            return false;
        }

        protected virtual void Erase()
        {
            currentEvent.Use();
            TryEraseAt(ObjectBrush.cellPosition);
        }
    }
}