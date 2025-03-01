using Character;
using System.Collections;
using UnityEngine;

public class PawnAnimator : EntityAnimator
{
    protected PawnEntity pawn;
    protected SpriteRenderer spriteRenderer;

    [AutoHash("up")]
    protected int id_up;

    public bool up;
    public bool right;

    public void Play(string movementName, float latency)
    {
        StartCoroutine(DelayPlay(movementName, latency));
    }

    private IEnumerator DelayPlay(string movementName, float latency)
    {
        animator.Play("Idle");
        yield return new WaitForSeconds(latency);
        animator.Play(movementName);
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = (PawnEntity)entity;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        up = pawn.faceDirection.x + pawn.faceDirection.y > 0;
        right = pawn.faceDirection.x - pawn.faceDirection.y > 0;
        animator.SetBool(id_up, up);
        transform.localScale = right ? new Vector3(-1, 1, 1) : Vector3.one;
    }
}
