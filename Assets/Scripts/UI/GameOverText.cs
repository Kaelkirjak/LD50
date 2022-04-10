using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{
    [SerializeField] TMP_Text gameOverText;

    void OnStateChanged(LevelState newState)
    {
        if (newState == LevelState.GameOver)
        {
            gameOverText.enabled = true;
        }
        else
        {
            gameOverText.enabled = false;
        }
    }

    private void Start()
    {
        LevelManager.instance.onLevelStateChange += OnStateChanged;
    }
    private void OnDestroy()
    {
        LevelManager.instance.onLevelStateChange -= OnStateChanged;
    }
}
