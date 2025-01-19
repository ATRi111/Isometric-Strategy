using MyTool;
using System.Text;

[System.Serializable]
public class BuffPreCondition
{
    public BuffSO so;
    public bool requireExist;

    public bool Verify(PawnEntity pawn)
    {
        bool exist = pawn.BuffManager.FindEnabled(so) != null;
        return exist == requireExist;
    }

    public void Describe(StringBuilder sb)
    {
        sb.Append(requireExist ? "����" : "δ����");
        sb.Append(so.name.Bold());
        sb.Append("״̬��");
        sb.AppendLine();
    }
}