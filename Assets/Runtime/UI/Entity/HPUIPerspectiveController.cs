public class HPUIPerspectivController : PerspectiveController
{
    private HPUI hpUI;

    protected override bool CoverCheck()
    {
        if(hpUI.entity == null)
            return false;
        return perspectiveManager.CoverCheck(hpUI.entity.GridObject.CellPosition);
    }

    protected override void Awake()
    {
        base.Awake();
        hpUI = GetComponent<HPUI>();
    }
}
