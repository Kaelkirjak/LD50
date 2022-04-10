using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualHelper : MonoBehaviour
{
    [SerializeField] SpriteRenderer renderer;

    void OnScored(int newScore)
    {
        renderer.enabled = false;
    }

    void OnGameStarted(LevelState newState)
    {
        if(newState == LevelState.Playing)
        {
            renderer.enabled = true;
        }
        else
        {
            renderer.enabled = false;
        }
    }

    private void Start()
    {
        LevelManager.instance.onScoreChanged += OnScored;
        LevelManager.instance.onLevelStateChange += OnGameStarted;
    }

    private void OnDestroy()
    {
        LevelManager.instance.onScoreChanged -= OnScored;
        LevelManager.instance.onLevelStateChange -= OnGameStarted;
    }
}
