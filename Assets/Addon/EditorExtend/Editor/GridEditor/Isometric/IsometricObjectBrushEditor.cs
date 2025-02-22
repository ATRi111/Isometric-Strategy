using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        public new IsometricObjectBrush ObjectBrush => target as IsometricObjectBrush;
        [AutoProperty]
        public SerializedProperty pillarMode, lockLayer, layer, lockXY;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            pillarMode.BoolField("���λ���ģʽ");
            lockXY.BoolField("����XY");
            lockLayer.BoolField("��������");
            if(lockLayer.boolValue)
            {
                layer.IntField("����");
            }
            EditorGUILayout.HelpBox("��סCtrl����XY;��סShift��������", MessageType.Info);
        }

        protected override void Brush()
        {
            base.Brush();
            if(ObjectBrush.pillarMode)
            {
                GridObject gridObject = ObjectBrush.prefab.GetComponent<GridObject>();
                if (gridObject.IsGround)
                {
                    Vector3Int position = ObjectBrush.cellPosition + Vector3Int.back;
                    for (; position.z >= 0; position += Vector3Int.back)
                    {
                        TryBrushAt(position);
                    }
                }
            }
        }

        protected override void Erase()
        {
            base.Erase(); 
            if (ObjectBrush.pillarMode && lockLayer.boolValue)
            {
                GridObject gridObject = ObjectBrush.prefab.GetComponent<GridObject>();
                if (gridObject.IsGround)
                {
                    Vector3Int position = ObjectBrush.cellPosition + Vector3Int.back;
                    for (; position.z >= 0; position += Vector3Int.back)
                    {
                        TryEraseAt(position);
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                currentEvent.Use();
                UpdateCellPosition();
                lockXY.boolValue = true;
                lockLayer.boolValue = false;
                lockedPosition = ObjectBrush.cellPosition;
            }
            else if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
            {
                currentEvent.Use();
                UpdateCellPosition();
                lockLayer.boolValue = true;
                layer.intValue = ObjectBrush.cellPosition.z;
                lockXY.boolValue = false;
                lockedPosition = ObjectBrush.cellPosition;
            }
        }
        protected override void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                currentEvent.Use();
                lockXY.boolValue = false;
            }
            else if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
            {
                currentEvent.Use();
                lockLayer.boolValue = false;
            }
        }
    }
}