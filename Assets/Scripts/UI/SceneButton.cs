using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : BaseButton
{
    [SerializeField][Tooltip("Scene to switch to")]                     GameScene sceneToSwitchTo;

    protected override void OnButtonClicked()
    {
        GameManager.instance.ChangeScene(sceneToSwitchTo);
    }
}
