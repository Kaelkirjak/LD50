using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanelButton : BaseButton
{
    [SerializeField][Tooltip("What panel to switch on to")]                 MenuPanel panelToSwitchTo;

    protected override void OnButtonClicked()
    {
        MenuManager.instance.ChangePanel(panelToSwitchTo);
    }
}
