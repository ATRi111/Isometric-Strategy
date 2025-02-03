using Character;

public class PawnAnimator : EntityAnimator
{
    private PawnEntity pawn;

    [AutoHash("x")]
    protected int id_x;
    [AutoHash("y")]
    protected int id_y;

    protected override void Awake()
    {
        base.Awake();
        pawn = (PawnEntity)entity;
    }

    private void Update()
    {
        animator.SetInteger(id_x, pawn.faceDirection.x);
        animator.SetInteger(id_y, pawn.faceDirection.y);
    }
}
