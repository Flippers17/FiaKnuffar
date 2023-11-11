using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _hitsSounds = new List<AudioClip>();
    [SerializeField] private AudioSource _source;

    public static HitSoundManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Too many HitSoundManager in scene!");
            Destroy(gameObject);
        }
    }

    public void PlayRandomHitSounds()
    {
        AudioClip clip = _hitsSounds[Random.Range(0, _hitsSounds.Count)];

        _source.PlayOneShot(clip);
    }
}
