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

    public void Register()
    {
        for(int i = 0;i < modifiers.Count;i++)
        {
            modifiers[i].Register();
        }
    }

    public void Unregister()
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].Unregister();
        }
    }
}
