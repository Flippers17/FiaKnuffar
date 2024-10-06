using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _uiText;

    [SerializeField]
    private float _timeShown = 5f;
    private float _timeSinceShown = 0;
    private bool _beingShown = false;

    public void InititatePopUp(string msg, float timeShown)
    {
        if (!_beingShown)
        {
            _timeShown = timeShown;
            _beingShown = true;
            _uiText.text = msg;
        }
        else
        {
            _timeShown = timeShown;
            _beingShown = true;
            _timeSinceShown = 0;
            _uiText.text = msg;
        }
    }

    private void Update()
    {
        if (_beingShown)
        {
            if (_timeSinceShown < _timeShown)
            {
                _timeSinceShown += Time.deltaTime;
            }
            else
            {
                _beingShown = false;
                _timeSinceShown = 0;
                _uiText.text = "";
            }
        }
    }
}
