using MyTool;
using System.Text;

[System.Serializable]
public class BuffModifier
{
    public BuffSO so;
    public int probability = Effect.MaxProbability;

    public void Describe(StringBuilder sb, string victimName)
    {
        if (probability != Effect.MaxProbability)
        {
            sb.Append("有");
            sb.Append(probability);
            sb.Append("%的几率");
        }
        sb.Append("使");
        sb.Append(victimName);
        sb.Append("获得时长为");
        sb.Append(so.duration);
        sb.Append("的");
        sb.Append(so.name.Bold());
        sb.AppendLine();
    }
}
