using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        [AutoProperty]
        public SerializedProperty lockLayer, lockedLayer;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            lockLayer.BoolField("Ëø¶¨²ãÊý");
            if(lockLayer.boolValue)
            {
                lockedLayer.IntField("²ãÊý");
            }
        }
    }
}