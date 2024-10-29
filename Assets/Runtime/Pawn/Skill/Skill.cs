using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public int actionTime;

    /// <summary>
    /// ��ȡ��ѡ�ļ���ʩ��λ��
    /// </summary>
    public virtual void GetOptions(Vector2Int position, List<Vector2Int> ret)
    {
        ret.Clear();
    }

    /// <summary>
    /// ģ�⼼�ܿ��ܲ�����Ч��
    /// </summary>
    public virtual void Mock(PawnEntity agent, Vector2Int target,IsometricGridManager igm, EffectUnit ret)
    {
        ret.timeEffect.current += actionTime;
    }
}
