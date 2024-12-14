using System;
using UnityEngine;

public class PerspectiveController : MonoBehaviour
{
    public Action EnterPerspectiveMode;
    public Action ExitPerspectiveMode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            EnterPerspectiveMode?.Invoke();
        else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            ExitPerspectiveMode?.Invoke();
    }
}
