using Services;
using Services.Event;
using TMPro;
using UIExtend;
using UnityEngine;

public class PawnPanel : MonoBehaviour
{
    private IEventSystem eventSystem;
    private PlayerManager playerManager;

    private CanvasGroupPlus canvasGroup;
    [SerializeField]
    private TextMeshProUGUI entityNameTmp;

    private int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            selectedIndex = value;
            entityNameTmp.text = playerManager.playerList[value].prefab.name;
        }
    }

    private void Show()
    {
        canvasGroup.Visible = true;
    }

    private void Hide()
    {
        canvasGroup.Visible = false;
    }


    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.ShowPawnPanel, Show);
    }

    private void OnDisable()
    {
        eventSystem.RemoveListener(EEvent.HidePawnPanel, Hide);
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponent<CanvasGroupPlus>();
        playerManager = PlayerManager.FindInstance();
        SelectedIndex = 0;
    }
}
