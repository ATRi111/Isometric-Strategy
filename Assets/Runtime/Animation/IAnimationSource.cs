/// <summary>
/// 决定动画过程中的参数的原始数据
/// </summary>
public interface IAnimationSource
{
    /// <summary>
    /// 动画播放完毕后应用效果
    /// </summary>
    void Apply();
}
