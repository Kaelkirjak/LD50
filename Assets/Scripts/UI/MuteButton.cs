using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : BaseButton
{
    [SerializeField][Tooltip("Sound type to mute")]             SoundType soundToMute;
    [SerializeField][Tooltip("Button muted state")]             Sprite mutedButton;
    [SerializeField][Tooltip("Button unmuted state")]           Sprite unmutedButton;

    protected override void OnButtonClicked()
    {
        GameManager.instance.ToggleVolumeMute(soundToMute);
        if (GameManager.instance.volumeMutes[soundToMute])
        {
            button.image.sprite = mutedButton;
        }
        else
        {
            button.image.sprite = unmutedButton;
        }
    }
}
