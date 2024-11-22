using UnityEngine;

[CreateAssetMenu(fileName = "��״̬", menuName = "״̬")]
public class Buff : PawnPropertyModifierSO
{
    public string buffName;
    public int duration;

    public virtual void Tick(int startTime, int currentTime)
    {

    }
}
