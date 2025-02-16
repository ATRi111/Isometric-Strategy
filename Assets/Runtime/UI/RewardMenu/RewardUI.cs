using Services;
using Services.Event;
using UIExtend;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
    private IEventSystem eventSystem;
    private CanvasGroupPlus canvasGroup;
    public Reward reward;

    public void Show()
    {
        canvasGroup.Visible = true; 
    }

    private void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        canvasGroup = GetComponentInChildren<CanvasGroupPlus>();
        if(reward != null)
        {
            NewPlayerIcon playerIcon = GetComponentInChildren<NewPlayerIcon>();
            if (reward.newPlayer != null)
                playerIcon.SetPlayer(reward.newPlayer.GetComponent<PawnEntity>());

            NewEquipmentIcon[] equipmentIcons = GetComponentsInChildren<NewEquipmentIcon>();
            int i = 0;
            for (; i < reward.equipmentList.Count; i++)
            {
                equipmentIcons[i].SetEquipment(reward.equipmentList[i]);
            }
            for (; i < equipmentIcons.Length; i++)
            {
                equipmentIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }

    private void OnDisable()
    {
        eventSystem.AddListener(EEvent.AfterBattle, Show);
    }
}
