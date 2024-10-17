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
            maxLayer.IntField("��߲���");
            minLayer.IntField("��Ͳ���");
        }
    }
}