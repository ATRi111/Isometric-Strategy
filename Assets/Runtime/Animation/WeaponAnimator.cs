using Services.ObjectPools;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WeaponAnimator : MonoBehaviour
{
    public static string BaseLayer = "BaseLayer.";

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public int extraSortingOrder;

    public void Play(string movementName, int sortingOrder)
    {
        spriteRenderer.sortingOrder = sortingOrder + extraSortingOrder;
        animator.Play(BaseLayer + movementName);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float t = stateInfo.length;
        StartCoroutine(DelayRecycle(t));
    }

    private IEnumerator DelayRecycle(float delay)
    {
        yield return new WaitForSeconds(delay);
        GetComponent<MyObject>().Recycle();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
