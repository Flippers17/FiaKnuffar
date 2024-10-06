using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private PlayerPush _playerPush;
    [SerializeField] private RandomAudioPlayer _audioPlayer;

    public void PushEnemy()
    {
        _playerPush.PushEnemy();
    }

    public void RightStep()
    {
        _audioPlayer.PlayRandomSound();
    }
    public void LeftStep()
    {
        _audioPlayer.PlayRandomSound();
    }
}
