using AStar;
using Character;
using Services;
using System.Collections.Generic;
using UnityEngine;

public class PawnBrain : CharacterComponentBase
{
    public PawnEntity Pawn => entity as PawnEntity;

    #region 决策
    public virtual float Evaluate(EffectUnit effectUnit)
    {
        return 0f;
    }
    #endregion

    #region 寻路
    public AIManager AIManager { get; private set; }

    public void FindAvalable(Vector2Int from, List<Vector2Int> ret)
    {
        ret.Clear();
        MovableGridObject gridObject = Pawn.GridObject;
        PathFindingProcess process = AIManager.PathFinding.FindAvailable(gridObject.Mover, from);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add(process.available[i].Position);
        }
    }

    public void FindRoute(Vector2Int from,Vector2Int to, List<Vector3Int> ret)
    {
        ret.Clear();
        MovableGridObject gridObject = Pawn.GridObject;
        PathFindingProcess process = AIManager.PathFinding.FindRoute(gridObject.Mover, from, to);
        ret.Add(gridObject.CellPosition);
        for (int i = 0; i < process.output.Count; i++)
        {
            ret.Add((process.output[i] as ANode).CellPositon);
        }
    }
    #endregion

    #region 技能
    [SerializeField]
    private SerializedHashSet<Skill> learnedSkills;

    public void Learn(Skill skill)
    {
        learnedSkills.Add(skill);
    }
    public bool IsLearned(Skill skill)
    {
        return learnedSkills.Contains(skill);
    }
    public bool Forget(Skill skill)
    {
        return learnedSkills.Remove(skill);
    }
    #endregion


    protected override void Awake()
    {
        base.Awake();
        learnedSkills = new();
        AIManager = ServiceLocator.Get<AIManager>();
    }
}
