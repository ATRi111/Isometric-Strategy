namespace Services.Event
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
        /// 地图初始化完成后
        /// </summary>
        AfterMapInitialize,
        /// <summary>
        /// 卸载场景前，参数：即将卸载的场景号
        /// </summary>
        BeforeUnLoadScene,
        /// <summary>
        /// 卸载场景后（至少一帧以后），参数：刚卸载完的场景号
        /// </summary>
        AfterUnLoadScene,
        /// <summary>
        /// 角色行动前，参数：PawnEntity
        /// </summary>
        BeforeDoAction,
        /// <summary>
        /// 角色开始行动时，参数：PawnAction
        /// </summary>
        OnDoAction,
        /// <summary>
        /// 天气发生变化后，参数：BattleField
        /// </summary>
        AfterWeatherChange,
        /// <summary>
        /// 显示信息，参数：信息内容,信息框和鼠标间距离(用屏幕宽度的百分比表示)
        /// </summary>
        ShowInfo,
        /// <summary>
        /// 隐藏信息，参数：引发事件的对象
        /// </summary>
        HideInfo,
        /// <summary>
        /// 显示二级信息，参数：屏幕坐标，信息内容
        /// </summary>
        ShowSecondaryInfo,
        /// <summary>
        /// 隐藏二级信息
        /// </summary>
        HideSecondaryInfo,
        /// <summary>
        /// 战斗开始前
        /// </summary>
        BeforeBattle,
        /// <summary>
        /// 战斗结束后，参数:战斗是否胜利
        /// </summary>
        AfterBattle,
        /// <summary>
        /// 时间变动，参数：当前全局时间
        /// </summary>
        OnTick,
        /// <summary>
        /// 显示角色面板，参数：Pawn
        /// </summary>
        ShowPawnPanel,
        /// <summary>
        /// 将一个角色添加到交换位置对中，参数:Pawn
        /// </summary>
        SwitchPawn,
        /// <summary>
        /// 将角色设为观察目标，参数：Pawn
        /// </summary>
        SetPawnTaregt,
        /// <summary>
        /// 隐藏角色面板
        /// </summary>
        HidePawnPanel,
        /// <summary>
        /// 输出战斗日志，参数：日志内容
        /// </summary>
        BattleLog,
    }
}