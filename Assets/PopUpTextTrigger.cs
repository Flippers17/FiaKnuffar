using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextTrigger : MonoBehaviour
{
    [SerializeField]
    private string _message = "";

    [SerializeField]
    private TextMeshProUGUI _uiText;

    [SerializeField]
    private float _timeShown = 5f;
    private float _timeSinceShown = 0;
    private bool _beingShown = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _beingShown = true;
            _uiText.text = _message;
        }
    }

    private void Update()
    {
        if (_beingShown)
        {
            if(_timeSinceShown < _timeShown)
            {
                _timeSinceShown += Time.deltaTime;
            }
            else
            {
                _uiText.text = "";
            }
        }
    }
}
