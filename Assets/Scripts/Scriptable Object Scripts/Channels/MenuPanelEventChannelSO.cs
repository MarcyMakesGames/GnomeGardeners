using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/MenuPanel Event Channel")]
public class MenuPanelEventChannelSO : ScriptableObject
{
    public delegate void MenuPanelAction(MenuPanel panel);
    public MenuPanelAction OnEventRaised;

    public void RaiseEvent(MenuPanel panel)
    {
        OnEventRaised?.Invoke(panel);
    }
}
