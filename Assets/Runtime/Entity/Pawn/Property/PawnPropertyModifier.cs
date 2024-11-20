using Character;
using System.Collections.Generic;

[System.Serializable]
public class PawnPropertyModifier
{
    public List<PropertyModifier> modifiers;

    public void Bind(PawnEntity pawn)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            FindPawnPropertySO ppm = modifiers[i].so as FindPawnPropertySO;
            ppm.pawn = pawn;
            modifiers[i].Bind();
        }
    }
}
