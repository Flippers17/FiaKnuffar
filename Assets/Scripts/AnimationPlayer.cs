using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animation _anim;

    public void Play()
    {
        _anim.Play();
    }
}
