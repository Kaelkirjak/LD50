using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSlider : MonoBehaviour
{
    [SerializeField][Tooltip("Connected slider")]               protected Slider slider;

    /// <summary>
    /// Function that is called when the connected sliders value is changed
    /// </summary>
    /// <param name="newValue"></param>
    protected virtual void OnSliderValueChanged(float newValue)
    {

    }

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
