using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : BaseSlider
{
    [SerializeField][Tooltip("Volume type that is connected with slider")]                  SoundType soundType;

    protected override void OnSliderValueChanged(float newValue)
    {
        GameManager.instance.ChangeVolume(soundType, newValue);
    }

    /// <summary>
    /// Change the sliders position
    /// </summary>
    /// <param name="soundTypeChanged"></param>
    void ChangeSliderValue(SoundType soundTypeChanged)
    {
        if(soundTypeChanged == soundType)
        {
            slider.value = GameManager.instance.GetVolume(soundType);
        }
    }

    private void Start()
    {
        GameManager.instance.onVolumeChanged += ChangeSliderValue;
        ChangeSliderValue(soundType);
    }

    private void OnDestroy()
    {
        GameManager.instance.onVolumeChanged -= ChangeSliderValue;
    }
}
