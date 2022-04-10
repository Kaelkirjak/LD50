using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayResetButton : BaseButton
{
    [SerializeField][Tooltip("Play state text")]            string playText;
    [SerializeField][Tooltip("Menu state text")]            string menuText;
    [SerializeField][Tooltip("Button text component")]      TMP_Text buttonText;

    protected override void OnButtonClicked()
    {
        if (LevelManager.instance.currentState == LevelState.Paused || LevelManager.instance.currentState == LevelState.GameOver || LevelManager.instance.currentState == LevelState.Default)
        {
            LevelManager.instance.StartLevel();
        }
        else
        {
            LevelManager.instance.ReturnToMenu();
        }
    }

    protected override void Start()
    {
        base.Start();
        LevelManager.instance.onLevelStateChange += ChangeText;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        LevelManager.instance.onLevelStateChange -= ChangeText;
    }

    void ChangeText(LevelState newState)
    {
        if(newState == LevelState.Playing)
        {
            buttonText.text = playText;
        }
        else
        {
            buttonText.text = menuText;
        }
    }
}
