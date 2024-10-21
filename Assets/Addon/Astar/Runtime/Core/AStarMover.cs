using System;

namespace AStar
{
    [Serializable]
    public class AStarMover
    {
        [NonSerialized]
        protected PathFindingProcess process;
        public float moveAbility;
        
        public AStarMover(PathFindingProcess process, float moveAbility = float.PositiveInfinity)
        {
            this.process = process;
            this.moveAbility = moveAbility;
        }

        public virtual bool MoveCheck(AStarNode from, AStarNode to)
        {
            return !to.IsObstacle;
        }

        public virtual float CalculateCost(AStarNode from, AStarNode to)
        {
            float primitive = from.PrimitiveCostTo(to);
            return primitive;
        }
    }
}