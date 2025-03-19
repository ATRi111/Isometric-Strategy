using Services;
using System;
using UnityEngine;

public class ScreenManager : Service ,IService
{
    public override Type RegisterType => GetType();

    public float aspectRatio;

    protected override void Awake()
    {
        base.Awake();
        Resolution resolution = Screen.currentResolution;
        float width = resolution.width;
        float height = resolution.height;
        width = Mathf.Min(width, aspectRatio * height);
        height = width / aspectRatio;
        Screen.SetResolution((int)width, (int)height, true);
    }
}
