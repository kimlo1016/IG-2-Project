using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerScrollButton : MonoBehaviour
{
    private SwitchControllerScrollUI _switchControllerScrollUI;
    private Defines.ESwitchController _type = Defines.ESwitchController.Left;
    private Dictionary<Defines.ESwitchController, VoiceTypeDelegate> _voiceTable = new Dictionary<Defines.ESwitchController, VoiceTypeDelegate>();

    [SerializeField]
    private GameObject _player;

    public Defines.ESwitchController Type
    {
        get
        {
            return _type;
        }
        private set
        {
            _type = value;
            _switchControllerScrollUI.TextOutput(_type);
        }
    }

    private delegate void VoiceTypeDelegate();

    private void Awake()
    {
        _switchControllerScrollUI = GetComponent<SwitchControllerScrollUI>();
        Type = Defines.ESwitchController.Left;

        _voiceTable.Add(Defines.ESwitchController.Left, ControllerTypeLeft);
        _voiceTable.Add(Defines.ESwitchController.Right, ControllerTypeRight);
    }

    public void OnClickLeftButton()
    {
        if (Type - 1 < Defines.ESwitchController.Left)
        {
            return;
        }
        --Type;
        _voiceTable[Type].Invoke();
    }

    public void OnClickRightButton()
    {
        if (Type + 1 >= Defines.ESwitchController.End)
        {
            return;
        }
        ++Type;
        _voiceTable[Type].Invoke();
    }

    private void ControllerTypeLeft()
    {
        Debug.Log("��������");
        _player.GetComponent<PlayerMovementLeft>().enabled = true;
        _player.GetComponent<PlayerMovementRight>().enabled = false;
    }

    private void ControllerTypeRight()
    {
        Debug.Log("����������");
        _player.GetComponent<PlayerMovementLeft>().enabled = false;
        _player.GetComponent<PlayerMovementRight>().enabled = true;
    }

}
