using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerPush _playerPush;

    public void PushEnemy()
    {
        _playerPush.PushEnemy();
    }
}
