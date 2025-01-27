using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PrivateRoomRadialMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _privateRoomRadialMenu;
    [SerializeField] Image _privateRoomRadialCursor;
    [SerializeField] GameObject _dice;
    //[SerializeField] 
    [SerializeField] GameObject _paintbrush;
    [SerializeField] Button _buttonDice;

    public static Button ClickButton;

    private SpawnDice _spawnDice;
    private SpawnPaintbrush _spawnPaintbrush;

    private Vector2 _priavteRoomRadialCursorInitPosition;
    private float _privateRoomRadialCursorMovementLimit = 25f;
    private float _privateRoomRadialCursorSpeed = 100f;

    private void Start()
    {
        _priavteRoomRadialCursorInitPosition = _privateRoomRadialCursor.rectTransform.localPosition;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("����");

        if (PhotonNetwork.IsMasterClient)
        {
            _dice = PhotonNetwork.Instantiate("Dice", transform.position, transform.rotation);
            _dice.SetActive(false);
        }
        else
        {
            _buttonDice.interactable = false;
        }

        //

        _paintbrush = PhotonNetwork.Instantiate("Paintbrush", transform.position, transform.rotation);
        _paintbrush.SetActive(false);

        _spawnDice = _dice.GetComponent<SpawnDice>();
        //
        _spawnPaintbrush = _paintbrush.GetComponent<SpawnPaintbrush>();
    }

    private void Update()
    {
        /*
        if (!photonView.IsMine)
        {
            return;
        }

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _privateRoomRadialMenu.SetActive(true);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
        {
            photonView.RPC("CallMethod", RpcTarget.All);
        }
        else
        {
            _privateRoomRadialMenu.SetActive(false);
        }
        */
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(ClickButton.name); 
            CallMethod();
        }
    }

    [PunRPC]
    private void CallMethod()
    {
        Debug.Log("Call");
        if(ClickButton.name == "ButtonA")
        {
            ButtonAMethod();
        }
        else if(ClickButton.name == "ButtonB")
        {
            ButtonBMethod();
        }
        else
        {
            ButtonCMethod();
        }
    }

    private void ButtonAMethod()
    {
        //if(!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}
        Debug.Log("dice");
        _spawnDice.ToggleDice();
    }

    private void ButtonBMethod()
    {

    }

    private void ButtonCMethod()
    {
        if (!_spawnPaintbrush.photonView.IsMine)
        {
            return;
        }
        Debug.Log("paint");
        _spawnPaintbrush.SpawnHelper();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MoveCursor();
        }
        else
        {
            ResetCursor();
        }
    }


    void MoveCursor()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        direction.Normalize();


        _privateRoomRadialCursor.rectTransform.localPosition = Vector3.ClampMagnitude(_privateRoomRadialCursor.rectTransform.localPosition + direction * _privateRoomRadialCursorSpeed * Time.deltaTime, _privateRoomRadialCursorMovementLimit);

    }

    void ResetCursor()
    {
        _privateRoomRadialCursor.rectTransform.localPosition = _priavteRoomRadialCursorInitPosition;
    }
}
