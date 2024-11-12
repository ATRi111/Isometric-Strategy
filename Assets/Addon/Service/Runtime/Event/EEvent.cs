namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// 加载场景前，参数：即将加载的场景号
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// 加载场景后（至少一帧以后），参数：刚加载好的场景号
        /// </summary>
        AfterLoadScene,
        /// <summary>
        /// 卸载场景前，参数：即将卸载的场景号
        /// </summary>
        BeforeUnLoadScene,
        /// <summary>
        /// 卸载场景后（至少一帧以后），参数：刚卸载完的场景号
        /// </summary>
        AfterUnLoadScene,
        /// <summary>
        /// 玩家开始控制某个角色，参数：控制的角色
        /// </summary>
        OnHumanControl,
        /// <summary>
        /// 显示信息，参数：引发事件的脚本，屏幕位置，信息内容
        /// </summary>
        ShowMessage,
        /// <summary>
        /// 隐藏信息，参数：引发事件的脚本
        /// </summary>
        HideMessage,
        /// <summary>
        /// 战斗开始
        /// </summary>
        BeforeBattle,
        /// <summary>
        /// 时间变动，参数：当前全局时间
        /// </summary>
        OnTick,
        /// <summary>
        /// 角色即将行动，参数：角色，当前全局时间
        /// </summary>
        BeforeDoAction,
    }
}