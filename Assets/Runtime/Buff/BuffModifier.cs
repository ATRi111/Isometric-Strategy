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
            sb.Append("��");
            sb.Append(probability);
            sb.Append("%�ļ���");
        }
        sb.Append("ʹ");
        sb.Append(victimName);
        sb.Append("���ʱ��Ϊ");
        sb.Append(so.duration);
        sb.Append("��");
        sb.Append(so.name.Bold());
        sb.AppendLine();
    }
}
