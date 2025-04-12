using Character;
using Services;
using Services.ObjectPools;
using System.Collections;
using UnityEngine;

public enum EPawnAnimationState
{
    Idle,
    Walk,
    Pierce,
    Slash,
    Raise,
    Static,
}

public class PawnAnimator : EntityAnimator
{
    public static string DownState = ".Down";

    protected IObjectManager objectManager;
    protected PawnEntity pawn;
    protected SpriteRenderer spriteRenderer;

    [AutoHash("up")]
    protected int id_up;

    public bool up;
    public bool right;
    [SerializeField]
    private Vector3 weaponOffset;

    public void Play(string movementName)
        => animator.Play(movementName + DownState);

    public void Play(string movementName, float latency, bool weaponAnimation)
    {
        StartCoroutine(DelayPlay(movementName, latency, weaponAnimation));
    }

    private IEnumerator DelayPlay(string movementName, float latency, bool weaponAnimation)
    {
        Play("Static");
        yield return new WaitForSeconds(latency);
        Play(movementName);
        if (weaponAnimation)
        {
            Equipment weapon = pawn.EquipmentManager.GetFirst(ESlotType.Weapon).equipment;
            if (weapon != null && weapon.animationName != null)
            {
                IMyObject obj = objectManager.Activate(weapon.animationName, transform.position, Vector3.zero, transform);
                obj.Transform.localPosition = weaponOffset;
                WeaponAnimator weaponAnimator = obj.Transform.GetComponent<WeaponAnimator>();
                weaponAnimator.Play(movementName, spriteRenderer.sortingOrder);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        pawn = (PawnEntity)entity;
        spriteRenderer = GetComponent<SpriteRenderer>();
        objectManager = ServiceLocator.Get<IObjectManager>();
    }

    protected virtual void Update()
    {
        up = pawn.faceDirection.x + pawn.faceDirection.y > 0;
        right = pawn.faceDirection.x - pawn.faceDirection.y > 0;
        animator.SetBool(id_up, up);
        transform.localScale = right ? new Vector3(-1, 1, 1) : Vector3.one;
    }
}
