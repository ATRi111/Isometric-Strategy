namespace AStar
{
    [System.Serializable]
    public class AStarMover
    {
        public float moveAbility;
        
        public AStarMover(float moveAbility = float.PositiveInfinity)
        {
            this.moveAbility = moveAbility;
        }

        public virtual bool MoveCheck(PathFindingProcess process, AStarNode from, AStarNode to)
        {
            return !to.IsObstacle;
        }
    }
}