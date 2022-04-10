using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [Tooltip("Button object")][SerializeField]                  protected Button button;

    /// <summary>
    /// Called when connected button is clicked
    /// </summary>
    protected virtual void OnButtonClicked()
    {

    }

    protected virtual void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
    }

    protected virtual void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonClicked);
    }
}
