using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _sounds;

    public void PlayRandomSound()
    {
        if (_sounds.Length <= 0)
            return;

        AudioClip randAudio = _sounds[Random.Range(0, _sounds.Length)];

        _audioSource.PlayOneShot(randAudio);
    }
}
