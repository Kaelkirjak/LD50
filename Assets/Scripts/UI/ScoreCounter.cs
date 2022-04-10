using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : BaseCounter
{
    private void Start()
    {
        LevelManager.instance.onScoreChanged += ChangeScore;
        ChangeScore(LevelManager.instance.currentScore);
    }

    private void OnDestroy()
    {
        LevelManager.instance.onScoreChanged -= ChangeScore;
    }

    void ChangeScore(int score)
    {
        ChangeText(score.ToString());
    }
}
