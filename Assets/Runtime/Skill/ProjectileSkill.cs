using EditorExtend.GridEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��һ����Χ���ͷ�,���е����ļ���
/// </summary>
public abstract class ProjectileSkill : RangedSkill
{
    //Ӧ����Mockʱ���´�
    public readonly List<Vector3> currentTrajectory = new();

    public override void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
        List<Vector3> trajectory = new();
        GridObject gridObject = HitCheck(agent, igm, target, trajectory);
        if(gridObject != null)
        {
            Entity entity = gridObject.GetComponentInParent<Entity>();
            if (entity != null && FilterVictim(entity))
                ret.Add(entity);
        }
    }

    /// <summary>
    /// ��ȡ��׼��
    /// </summary>
    /// <returns>��׼Ŀ���Ƿ����</returns>
    public virtual bool GetAimPoint(IsometricGridManager igm, Vector3Int target, out Vector3 to)
    {
        GridObject aimedObject = igm.GetObjectXY((Vector2Int)target);
        if (aimedObject == null)
        {
            to = target + 0.5f * Vector3.one;
            return false;
        }
        to = aimedObject.GetComponent<Entity>() != null ? aimedObject.Center : aimedObject.TopCenter;
        return true;
    }

    /// <summary>
    /// ���㼼�ܽ������е�Ŀ�꣬�����㵯��
    /// </summary>
    public GridObject HitCheck(PawnEntity agent, IsometricGridManager igm, Vector3Int target, List<Vector3> trajectory)
    {
        trajectory?.Clear();
        if (GetAimPoint(igm, target, out Vector3 to))
        {
            Vector3 from = agent.GridObject.Center;
            GridObject gridObject = HitCheck(igm, from, to, target, trajectory);
            return gridObject;
        }
        return null;
    }

    /// <summary>
    /// ���㼼�ܽ������е�Ŀ�꣬�����㵯��
    /// </summary>
    public abstract GridObject HitCheck(IsometricGridManager igm, Vector3 from, Vector3 to, Vector3Int target, List<Vector3> trajectory);
}