using UIExtend;
using UnityEngine;

public class PawnPanel : MonoBehaviour
{
    private CanvasGroupPlus canvasGroup;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }
}
