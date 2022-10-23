using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{


    [SerializeField] GameObject _radialMenu;
    [SerializeField] Image _cursor;
    [SerializeField] Image _feelingImage;
    [SerializeField] CircleCollider2D _cursorMovementLimit;
    public static Button _buttonOne;
    public static Image _buttonOneImage;
    private static Color _activeColor = new Color(1, 1, 1, 1);
    private static Color _deactiveColor = new Color(1, 1, 1, 0);
    private static readonly YieldInstruction _waitSecond = new WaitForSeconds(1f);

    private Vector2 _cursorInitPosition;
    private float _cursorSpeed = 100f;
    private float _coolTime = 4f;
    private void Start()
    {
        _cursorInitPosition = _cursor.rectTransform.localPosition;

    }

    void FadeOut()
    {


    }
    private void Update()
    {
        Debug.Log(_elapsedTime);
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _radialMenu.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            ButtonOneMenu();
        }
        else
        {
            _radialMenu.SetActive(false);
        }
    }

    private bool _isFadeRunning;
    float _elapsedTime = 0.0f;
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        _isFadeRunning = true;
        while (_elapsedTime < _coolTime)
        {
            _elapsedTime += Time.deltaTime;
            float animatedFadeAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(_elapsedTime / _coolTime));
            _feelingImage.color = new Color(1, 1, 1, animatedFadeAlpha);

            yield return new WaitForSeconds(0.0001f);
        }
        _feelingImage.color = new Color(1, 1, 1, endAlpha);
        _isFadeRunning = false;
        yield return null;

    }


    private void ButtonOneMenu()
    {
        if (_buttonOne != null)
        {
            _feelingImage.color = _activeColor;
            _feelingImage.sprite = _buttonOneImage.sprite;
            _radialMenu.SetActive(false);


            _elapsedTime = 0f;

            if (_isFadeRunning == false)
            {
                StartCoroutine(Fade(1, 0));
            }
        }
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


        _cursor.rectTransform.localPosition = Vector3.ClampMagnitude(_cursor.rectTransform.localPosition + direction * _cursorSpeed * Time.deltaTime, _cursorMovementLimit.radius);

    }

    void ResetCursor()
    {
        _cursor.rectTransform.localPosition = _cursorInitPosition;
    }

}