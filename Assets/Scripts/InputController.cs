using UnityEngine;
using System;

public class InputController : MonoBehaviour
{
    public event Action<Vector2Int> OnMove;
    public event Action OnEscapeButtonPressed;
    public event Action OnHomeButtonPressed;

    [SerializeField] private float _deadZone;

    private Vector2 _touchPosition;
    private Vector2 _touchDelta;
    private bool _isSwiping;
    private bool _isMobile;

    public void OnValidate()
    {
        if (_deadZone < 0)
            _deadZone = 0;
    }

     public void Start()
    {
        _isMobile = Application.isMobilePlatform;
    }

    public void Update()
    {
        if (!_isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isSwiping = true;
                _touchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    _isSwiping = true;
                    _touchPosition = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                    ResetSwipe();
            }
        }
        CheckSwipe();

        if (Input.GetKeyDown(KeyCode.Escape))
            OnEscapeButtonPressed?.Invoke();
        
        if (Input.GetKeyDown(KeyCode.Home))
            OnHomeButtonPressed?.Invoke();        
    }

    private void CheckSwipe()
    {
        _touchDelta = Vector2.zero;

        if (_isSwiping)
        {
            if(!_isMobile && Input.GetMouseButton(0))
                _touchDelta = (Vector2)Input.mousePosition - _touchPosition;
            else if (Input.touchCount > 0)
                _touchDelta = Input.GetTouch(0).position - _touchPosition;
        }

        if (_touchDelta.magnitude > _deadZone)
        {
            RoundDirection();
            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        _isSwiping = false;
        _touchPosition = Vector2.zero;
        _touchDelta = Vector2.zero;
    }

    private void RoundDirection()
    {
        _touchDelta.Normalize();

        if (_touchDelta.x > 0.2f)
            _touchDelta.x = 1;
        else if (_touchDelta.x < -0.2f)
            _touchDelta.x = -1;
        else
            return;

        if(_touchDelta.y > 0.2f)
            _touchDelta.y = 1;
        else if (_touchDelta.y < -0.2f)
            _touchDelta.y = -1;
        else
            _touchDelta.y = 0;

        Vector2Int direction = Vector2Int.CeilToInt(_touchDelta);
        OnMove?.Invoke(direction);
    }
}
