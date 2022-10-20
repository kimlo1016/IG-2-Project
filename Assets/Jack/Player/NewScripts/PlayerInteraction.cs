using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerInteraction : MonoBehaviourPun
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private PlayerFocus[] _playerFocus = new PlayerFocus[2];

    [SerializeField] private OVRGazePointer _pointer;
    private OVRInputModule _eventSystemInputModule;
    private OVRRaycaster _ovrRaycaster;

    private void OnEnable()
    {
        if(photonView.IsMine)
        {
            _ovrRaycaster = FindObjectOfType<OVRRaycaster>();
            _ovrRaycaster.pointer = _pointer.gameObject;

            _eventSystemInputModule = FindObjectOfType<OVRInputModule>();
            _eventSystemInputModule.m_Cursor = _pointer;
            _eventSystemInputModule.rayTransform = _playerFocus[0].gameObject.transform;
        }
    }

    private void Update()
    {
        if(_input.IsRay)
        {
            SettingUIInteraction();
            Interact();
        }
        else
        {
            _pointer.gameObject.SetActive(false);
        }
    }

    private void SettingUIInteraction()
    {
        if(_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            if(_input.IsLeftRay)
            {
                SetPointerTransform(_playerFocus[0]);
            }
            else
            {
                SetPointerTransform(_playerFocus[1]);
            }
        }
        else
        {
            if (_input.IsRightRay)
            {
                SetPointerTransform(_playerFocus[1]);
            }
            else
            {
                SetPointerTransform(_playerFocus[0]);
            }
        }

        _pointer.gameObject.SetActive(true);
    }
    private void SetPointerTransform(PlayerFocus playerFocus)
    {
        _pointer.rayTransform = playerFocus.gameObject.transform;
        _eventSystemInputModule.rayTransform = playerFocus.gameObject.transform;
    }

    private void Interact()
    {
        if(!_input.InputA)
        {
            return;
        }

        if (_input.PrimaryController == Defines.EPrimaryController.Left)
        {
            if (_input.IsLeftRay)
            {
                CheckAndInteract(_playerFocus[0]);
            }
            else
            {
                CheckAndInteract(_playerFocus[1]);
            }
        }
        else
        {
            if (_input.IsRightRay)
            {
                CheckAndInteract(_playerFocus[1]);
            }
            else
            {
                CheckAndInteract(_playerFocus[0]);
            }
        }
    }

    private void CheckAndInteract(PlayerFocus playerFocus)
    {
        if (playerFocus.HaveFocuseObject)
        {
            InteracterableObject interacterableObject = playerFocus.FocusedObject.gameObject.GetComponent<InteracterableObject>();
            if(interacterableObject)
            {
                interacterableObject.Interact();
            }
        }
    }
}