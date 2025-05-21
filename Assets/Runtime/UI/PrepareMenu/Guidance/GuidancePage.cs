using UIExtend;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupPlus))]
public class GuidancePage : MonoBehaviour
{
    [HideInInspector]
    public CanvasGroupPlus canvasGroup;
    public EGuidance guidance;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroupPlus>();
    }
}
