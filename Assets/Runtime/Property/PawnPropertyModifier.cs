using Character;
using System;
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
            FindPawnPropertySO ppm = modifiers[i].so as FindPawnPropertySO;
            ppm.pawn = pawn;
            modifiers[i].Register(ppm.FindProperty());
        }
    }

    public void Unregister(PawnEntity pawn)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            FindPawnPropertySO ppm = modifiers[i].so as FindPawnPropertySO;
            ppm.pawn = pawn;
            modifiers[i].Unregister(ppm.FindProperty());
        }
    }

    public void Describe(StringBuilder sb)
    {
        for (int i = 0; i < modifiers.Count; i++)
        {
            PropertyModifier modifier = modifiers[i];
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
                    sb.Append("��");
                    sb.Append((1f + modifier.value).ToString("P0"));
                    break;
            }
            sb.AppendLine();
        }
    }
}
