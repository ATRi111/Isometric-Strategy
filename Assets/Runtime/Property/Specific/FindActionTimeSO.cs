using Character;
using UnityEngine;

[CreateAssetMenu(fileName = "�����ж�ʱ��", menuName = "����/�����ж�ʱ��")]
public class FindActionTimeSO : FindPawnPropertySO
{
    public override CharacterProperty FindProperty()
    {
        return pawn.actionTime;
    }
}