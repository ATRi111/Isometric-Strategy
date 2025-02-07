using TMPro;
using UnityEngine;

public class SelectedCountUI : MonoBehaviour
{
    private PlayerManager playerManager;
    private TextMeshProUGUI tmp;

    private void AfterSelectChange()
    {
        tmp.text = $"������ѡ��{playerManager.MaxSelectedCount - playerManager.SelectedCount}λ��ɫ��ս";
    }

    private void Awake()
    {
        playerManager = PlayerManager.FindInstance();
        tmp = GetComponent<TextMeshProUGUI>();
        playerManager.AfterSelectChange += AfterSelectChange;
    }
}
