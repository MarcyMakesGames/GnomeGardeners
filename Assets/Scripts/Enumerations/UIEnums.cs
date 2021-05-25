using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GnomeGardeners
{
    public enum MenuPanel
    {
        Title,
        Main,
        Settings,
        GnomeSelection,
        Credits,
        Manual,
    }

    public enum SceneState
    {
        MainMenu,
        Game,
    }

    public enum InGameUIMode
    {
        HUD,
        PauseMenu,
        SettingsMenu,
        TutorialMenu,
        GameOverMenu,
    }
}
