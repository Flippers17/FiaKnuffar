using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private IntEventPort _updateHealth;

    [SerializeField]
    private TextMeshProUGUI _healthText;

    [SerializeField]
    private Slider slider;

    private int rectMod = 4;

    public QuickTimeType quickTimeType;

    private void OnEnable()
    {
        _quickTimeValueUpdate.OnInvoked += UpdateValue;
        _setGreenZoneEvent.OnInvoked += SetGreenZone;
        _updateHealth.OnInvoked += UpdateHealthText;
    }

    private void OnDisable()
    {
        _quickTimeValueUpdate.OnInvoked -= UpdateValue;
        _setGreenZoneEvent.OnInvoked -= SetGreenZone;
        _updateHealth.OnInvoked -= UpdateHealthText;
    }

    private void UpdateValue(float value)
    {
        if(quickTimeType == QuickTimeType.timing)
        {
            if (value > 100 || value < 1)
                return;

            slider.value = value;
        }
        else if(quickTimeType == QuickTimeType.mashing)
        {
            RectTransform thisRect = (RectTransform)transform;
            _greenZone.sizeDelta = new Vector2(value * rectMod, thisRect.rect.height);
            slider.value = value;
        }
    }
    
    private void SetGreenZone(int low, int high)
    {
        if(quickTimeType == QuickTimeType.timing)
        {
            RectTransform thisRect = (RectTransform)transform;

            rectMod = (int)thisRect.rect.width / 100;
            _greenZone.sizeDelta = new Vector2((high - low) * rectMod, thisRect.rect.height);
            _greenZone.anchoredPosition = new Vector2(low * rectMod, 0);
        }
        else if(quickTimeType == QuickTimeType.mashing)
        {
            RectTransform thisRect = (RectTransform)transform;

            rectMod = (int)thisRect.rect.width / 100;
            _greenZone.sizeDelta = new Vector2(0, thisRect.rect.height);
            _greenZone.anchoredPosition = new Vector2(0, 0);
        }
    }

    private void UpdateHealthText(int value)
    {
        _healthText.text = value + "X";
    }
}
