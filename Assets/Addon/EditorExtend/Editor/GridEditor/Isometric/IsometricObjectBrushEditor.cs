using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        public new IsometricObjectBrush ObjectBrush => target as IsometricObjectBrush;
        [AutoProperty]
        public SerializedProperty lockLayer, layer, lockXY;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            lockXY.BoolField("����XY");
            lockLayer.BoolField("��������");
            if(lockLayer.boolValue)
            {
                layer.IntField("����");
            }
            EditorGUILayout.HelpBox("��סCtrl����XY;��סShift��������", MessageType.Info);
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                currentEvent.Use();
                lockXY.boolValue = true;
                lockLayer.boolValue = false;
            }
            else if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
            {
                currentEvent.Use();
                lockLayer.boolValue = true;
                layer.intValue = ObjectBrush.cellPosition.z;
                lockXY.boolValue = false;
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