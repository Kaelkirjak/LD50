using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(LevelManager.instance.currentState == LevelState.Playing)
        { 
            LevelManager.instance.GameOver();
        }
    }
}
