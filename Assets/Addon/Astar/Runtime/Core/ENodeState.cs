namespace AStar
{
    public enum ENodeState
    {
        /// <summary>
        /// δ���ʹ��Ľڵ�
        /// </summary>
        Blank,
        /// <summary>
        /// ·��ȷ��������һ�εĽڵ�
        /// </summary>
        Close,
        /// <summary>
        /// ·����ȷ���Ľڵ�
        /// </summary>
        Open,
        /// <summary>
        /// ����·���еĽڵ�
        /// </summary>
        Route,
    }
}