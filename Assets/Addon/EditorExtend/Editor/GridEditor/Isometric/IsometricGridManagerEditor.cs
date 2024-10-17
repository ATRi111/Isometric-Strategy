using UnityEditor;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricGridManager),true)]
    public class IsometricGridManagerEditor : GridManagerEditor
    {
        [AutoProperty]
        public SerializedProperty maxLayer, minLayer;

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            maxLayer.IntField("最高层数");
            minLayer.IntField("最低层数");
        }
    }
}