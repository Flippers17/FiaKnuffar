using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpTextTrigger : MonoBehaviour
{
    [SerializeField][TextArea]
    private string _message = "";

    [SerializeField]
    private PopUpTextUI _uiText;

    [SerializeField]
    private float _timeShown = 5f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _uiText.InititatePopUp(_message, _timeShown);
        }
    }
}
