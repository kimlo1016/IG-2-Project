using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckPanelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _informationText;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _refuseButton;

    public delegate void CheckButtonAction();

    public void CheckPanelSetting(string informationMessage, 
        CheckButtonAction acceptAction, CheckButtonAction refuseAction)
    {
        _informationText.text = informationMessage;
        
        _acceptButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.AddListener(() => { acceptAction(); });

        _refuseButton.onClick.RemoveAllListeners();
        _refuseButton.onClick.AddListener(() => 
        { 
            refuseAction(); 
            gameObject.SetActive(false); 
        });
    }
}
