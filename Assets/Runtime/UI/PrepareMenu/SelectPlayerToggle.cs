using UIExtend;
using UnityEngine;

public class SelectPlayerToggle : ToggleBase
{
    private PlayerManager playerManager;

    [SerializeField]
    private int index;

    protected override void OnToggle(bool value)
    {
        if (value)
            playerManager.Select(index);
        else
            playerManager.Unselect(index);
    }

    private void AfterSelectChange()
    {
        Toggle.interactable = !playerManager.FullySelected || Toggle.isOn;
    }

    protected override void Awake()
    {
        base.Awake();
        playerManager = PlayerManager.FindInstance();
        playerManager.AfterSelectChange += AfterSelectChange;
    }
}
