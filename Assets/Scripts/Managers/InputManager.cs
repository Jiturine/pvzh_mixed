using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : PersistentSingleton<InputManager>
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.TogglePanel<OptionsPanel>();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
