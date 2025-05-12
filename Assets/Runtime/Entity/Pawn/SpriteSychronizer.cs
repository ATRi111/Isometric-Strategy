using UnityEngine;
using UnityEngine.Rendering;

public class SpriteSychronizer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer targetSpriteRenderer;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private SortingGroup sortingGroup;

    public void Sychronize()
    {
        spriteRenderer.sprite = targetSpriteRenderer.sprite;
        spriteRenderer.color = targetSpriteRenderer.color;
        sortingGroup.sortingOrder = spriteRenderer.sortingOrder;
    }

    private void LateUpdate()
    {
        Sychronize();
    }

    private void OnDrawGizmos()
    {
        if (spriteRenderer != null && targetSpriteRenderer != null && sortingGroup != null)
            Sychronize();
    }
}
