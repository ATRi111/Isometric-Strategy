using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    public static string BaseLayer = "BaseLayer.";

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public void Play(string movementName, int sortingOrder)
    {
        spriteRenderer.sortingOrder = sortingOrder;
        animator.Play(BaseLayer + movementName);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float t = stateInfo.length;
        Destroy(gameObject, t);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
