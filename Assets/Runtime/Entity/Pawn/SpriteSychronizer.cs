using UnityEngine;

public class SpriteSychronizer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer targetSpriteRenderer;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    public void Sychronize()
    {
        spriteRenderer.sprite = targetSpriteRenderer.sprite;
        spriteRenderer.color = targetSpriteRenderer.color;
    }

    private void LateUpdate()
    {
        Sychronize();
    }

    private void OnDrawGizmos()
    {
        if (spriteRenderer != null && targetSpriteRenderer != null)
            Sychronize();

        
    }
}
