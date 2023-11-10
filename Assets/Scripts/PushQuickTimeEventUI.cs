using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PushQuickTimeEventUI : MonoBehaviour
{
    [SerializeField]
    private FloatEventPort _quickTimeValueUpdate;
    [SerializeField]
    private TwoIntEventPort _setGreenZoneEvent;

    [SerializeField]
    private RectTransform _greenZone;

    [SerializeField]
    private Slider slider;

    private void OnEnable()
    {
        _quickTimeValueUpdate.OnInvoked += UpdateValue;
        _setGreenZoneEvent.OnInvoked += SetGreenZone;
    }

    private void OnDisable()
    {
        _quickTimeValueUpdate.OnInvoked -= UpdateValue;
        _setGreenZoneEvent.OnInvoked -= SetGreenZone;
    }

    private void UpdateValue(float value)
    {
        if (value > 100 || value < 1)
            return;

        slider.value = value;
    }
    
    private void SetGreenZone(int low, int high)
    {
        RectTransform thisRect = (RectTransform)transform;

        int rectMod = (int)thisRect.rect.width / 100;
        _greenZone.sizeDelta = new Vector2((high - low) * rectMod, thisRect.rect.height);
        _greenZone.anchoredPosition = new Vector2(low * rectMod, 0);
    }
}
