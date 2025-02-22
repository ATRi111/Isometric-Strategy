public class RewardUI : AfterBattleUI
{
    public Reward reward;

    protected override void Awake()
    {
        base.Awake();
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
}
