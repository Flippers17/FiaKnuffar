using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static float shakeDuration;
    private static float shakeMagnitude = .7f;
    private static float dampingSpeed = .4f;

    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    public static void TriggerShake(float duration, float _shakeMagnitude, float _dampingSpeed)
    {
        shakeDuration = duration;

        shakeMagnitude = _shakeMagnitude;

        dampingSpeed = _dampingSpeed;
    }

    private void Update()
    {
        if(shakeDuration > 0)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude, dampingSpeed);
            shakeDuration -= Time.deltaTime;
            if (shakeDuration <= 0)
            {
                shakeDuration = 0;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, .9f);
            }
        }
    }
}
