using System.Text;

[System.Serializable]
public class BuffModifier
{
    public BuffSO so;
    public bool remove;
    public int probability = Effect.MaxProbability;

    public void Describe(StringBuilder sb, string victimName)
    {
        if (probability != Effect.MaxProbability)
        {
            sb.Append("��");
            sb.Append(probability);
            sb.Append("%�ļ���");
        }
        if (!remove)
        {
            sb.Append("ʹ");
            sb.Append(victimName);
            sb.Append("���ʱ��Ϊ");
            if (so != null)
            {
                sb.Append(so.duration);
                sb.Append("��");
                sb.Append(so.name);
            }
        }
        else
        {
            sb.Append("�Ƴ�");
            sb.Append(victimName);
            sb.Append("��");
            if (so != null)
            {
                sb.Append(so.name);
            }
        }
        sb.AppendLine();
    }
}
