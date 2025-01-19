using Character;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class PawnPropertyModifier
{
    public List<PropertyModifier> modifiers;

    public void Register(PawnEntity pawn)
    {
        for(int i = 0;i < modifiers.Count;i++)
        {
            FindPawnPropertySO fpp = modifiers[i].so as FindPawnPropertySO;
            fpp.pawn = pawn;
            modifiers[i].Register(fpp.FindProperty());
        }
    }

    public void Unregister(PawnEntity pawn)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            FindPawnPropertySO fpp = modifiers[i].so as FindPawnPropertySO;
            fpp.pawn = pawn;
            modifiers[i].Unregister(fpp.FindProperty());
        }
    }

    public virtual void Describe(StringBuilder sb)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            PropertyModifier modifier = modifiers[i];
            if (modifier.so != null)
            {
                sb.Append(modifier.so.name);
                switch (modifier.method)
                {
                    case EModifyMethod.DirectAdd:
                    case EModifyMethod.FinalAdd:
                        if (Mathf.Abs(modifier.value) > 1)
                            sb.Append(modifier.value.ToString("+0.##;-0.##;0"));
                        else
                            sb.Append(modifier.value.ToString("+0%;-0%;0"));
                        break;
                    case EModifyMethod.DirectMultiply:
                    case EModifyMethod.FinalMultiply:
                        sb.Append("¡Á");
                        sb.Append((1f + modifier.value).ToString("P0"));
                        break;
                }
                sb.AppendLine();
            }
        }
    }
}
