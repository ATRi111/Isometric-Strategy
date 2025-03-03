using Character;
using System.Collections;
using UnityEngine;

public enum EPawnAnimationState
{
    Idle,
    Walk,
    Pierce,
    Slash,
    Raise,
}

public class PawnAnimator : EntityAnimator
{
    public static string BaseLayer = "BaseLayer.";

    protected PawnEntity pawn;
    protected SpriteRenderer spriteRenderer;

    [AutoHash("up")]
    protected int id_up;

    public bool up;
    public bool right;

    public void Play(string movementName, float latency, bool weaponAnimation)
    {
        StartCoroutine(DelayPlay(movementName, latency, weaponAnimation));
    }

    private IEnumerator DelayPlay(string movementName, float latency, bool weaponAnimation)
    {
        animator.Play(BaseLayer + "Idle");
        yield return new WaitForSeconds(latency);
        animator.Play(BaseLayer + movementName);
        if (weaponAnimation)
        {
            Equipment weapon = pawn.EquipmentManager.GetFirst(ESlotType.Weapon).equipment;
            if (weapon != null && weapon.animationPrefab != null)
            {
                GameObject obj = Instantiate(weapon.animationPrefab);
                obj.transform.SetParent(transform);
                obj.transform.localScale = Vector3.zero;
                WeaponAnimator weaponAnimator = obj.GetComponent<WeaponAnimator>();
                weaponAnimator.Play(movementName, spriteRenderer.sortingOrder + 1);
            }
        }
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
