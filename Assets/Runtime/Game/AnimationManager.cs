using Services;
using System;

public class AnimationManager : Service,IService
{
    public override Type RegisterType => GetType();

    public bool ImmediateMode { get; set; }
}
