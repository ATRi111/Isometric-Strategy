public class RewardUI : AfterBattleUI
{
    public Reward reward;

    protected override void Awake()
    {
        base.Awake();
        if (reward != null)
        {
            NewPlayerIcon[] playerIcons = GetComponentsInChildren<NewPlayerIcon>();
            NewEquipmentIcon[] equipmentIcons = GetComponentsInChildren<NewEquipmentIcon>();
            int i = 0;
            for (; i < reward.newPlayerList.Count; i++)
            {
                playerIcons[i].SetPlayer(reward.newPlayerList[i].GetComponent<PawnEntity>());
            }
            for (; i < playerIcons.Length; i++)
            {
                playerIcons[i].gameObject.SetActive(false);
            }
            for (i = 0; i < reward.equipmentList.Count; i++)
            {
                equipmentIcons[i].SetEquipment(reward.equipmentList[i]);
            }
            for (; i < equipmentIcons.Length; i++)
            {
                equipmentIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
