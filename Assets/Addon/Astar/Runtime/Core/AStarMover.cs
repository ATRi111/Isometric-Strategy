using System;
using UnityEngine;

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

        public virtual bool MoveAbilityCheck(AStarNode node)
        {
            return Mathf.RoundToInt(node.GCost) <= moveAbility;
        }

        public virtual bool StayCheck(AStarNode node)
        {
            return !node.IsObstacle;
        }

        public virtual bool MoveCheck(AStarNode from, AStarNode to)
        {
            return !to.IsObstacle;
        }

        public virtual float CalculateCost(AStarNode from, AStarNode to, float primitiveCost)
        {
            return primitiveCost;
        }
    }
}