using EditorExtend.GridEditor;
using MyTool;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public const int MaxAccuracy = 100;

    public int actionTime;
    public int accuracy = MaxAccuracy;

    public List<SkillPower> powers;

    public static int DistanceBetween(Vector3Int position, Vector3Int target)
    {
        return IsometricGridUtility.ProjectManhattanDistance((Vector2Int)position, (Vector2Int)target);
    }

    /// <summary>
    /// ��ȡ��ѡ�ļ���ʩ��λ��
    /// </summary>
    public virtual void GetOptions(PawnEntity agent, IsometricGridManager igm, Vector3Int position, List<Vector3Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// ��ȡ�������е�Entity
    /// </summary>
    public virtual void GetVictims(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, List<Entity> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// �ж�ĳ��entity�Ƿ��Ǽ��������Ŀ��
    /// </summary>
    public virtual bool FilterVictim(Entity entity)
    {
        return true;
    }

    /// <summary>
    /// ģ�⼼�ܲ�����Ӱ��
    /// </summary>
    public virtual void Mock(PawnEntity agent, IsometricGridManager igm, Vector3Int position, Vector3Int target, EffectUnit ret)
    {
        ret.timeEffect.current += MockTime(agent, position, target, igm);
        List<Entity> victims = new();
        GetVictims(agent, igm, position, target, victims);
        for (int i = 0; i < victims.Count; i++)
        {
            int r = RandomTool.GetGroup(ERandomGrounp.Battle).NextInt(1, Effect.MaxProbability + 1);
            int damage = 0;
            for (int j = 0; j < powers.Count; j++)
            {
                damage += victims[i].BattleComponent.MockDamage(powers[j].type, 100 * powers[j].power); //TODO:������
            }
            HPChangeEffect effect = new(victims[i], victims[i].BattleComponent.HP, victims[i].BattleComponent.HP - damage, accuracy)
            {
                randomValue = r
            };
            ret.effects.Add(effect);
        }
    }

    /// <summary>
    /// ģ�⼼�ܻ��ѵ�ʱ��
    /// </summary>
    public virtual int MockTime(PawnEntity agent, Vector3Int position, Vector3Int target, IsometricGridManager igm)
    {
        return actionTime;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine(name);
        return base.ToString();
    }
}
