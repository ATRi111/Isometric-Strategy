using System.Text;
using UnityEngine;

public abstract class SkillPrecondition : ScriptableObject
{
    public abstract bool Verify(IsometricGridManager igm, PawnEntity agent);

    public abstract void Describe(StringBuilder sb);
}
