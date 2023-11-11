using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private bool _destroyOnAwake = false;
    public float destroyTime = 3f;

    private void Awake()
    {
        if(_destroyOnAwake)
            StartDestroy();
    }

    public void StartDestroy()
    {
        StartCoroutine(PlayHitSound());
        Destroy(gameObject, destroyTime);
    }

    private IEnumerator PlayHitSound()
    {
        yield return new WaitForSeconds(destroyTime - 0.5f);
        HitSoundManager.Instance.PlayRandomHitSounds();
    }
}
