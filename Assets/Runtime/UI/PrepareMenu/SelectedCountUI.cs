using TMPro;
using UnityEngine;

public class SelectedCountUI : MonoBehaviour
{
    private PlayerManager playerManager;
    private TextMeshProUGUI tmp;

    private void AfterSelectChange()
    {
        tmp.text = $"还可以选择{playerManager.MaxSelectedCount - playerManager.SelectedCount}位角色出战";
    }

    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
        tmp = GetComponent<TextMeshProUGUI>();
        playerManager.AfterSelectChange += AfterSelectChange;
    }
}
