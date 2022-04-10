using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableButton : BaseButton
{
    [SerializeField] GameObject objectToDisable;

    protected override void OnButtonClicked()
    {
        objectToDisable.SetActive(false);
    }
}
