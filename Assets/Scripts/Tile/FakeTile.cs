using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FakeTile : MonoBehaviour
{
    public event Action<FakeTile> OnDestroyed;

    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _image;
    [SerializeField] private float _time;

    private float _speed;
    private bool _isStart=false;
    private Vector2 _targetPosition;
    private RectTransform _rectTransform;
    private float _distance => Vector2.Distance(transform.position, _targetPosition);
    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    public void Initialize(FakeTileConfiguration configuration)
    {
        InitializeTransform(configuration);
        InitializeView(configuration);
        InitializeData(configuration);
    }

    private void InitializeTransform(FakeTileConfiguration configuration)
    {
        transform.position = configuration.StartPosition;
        _rectTransform.sizeDelta = configuration.Scale;
    }

    private void InitializeView(FakeTileConfiguration configuration)
    {
        int value = configuration.Value;
        _image.color = Colors.instant.GetColor(value);
        _valueText.text = Mathf.Pow(2, value).ToString();
        _valueText.color = Colors.instant.GetTextColor(value);
    }

    private void InitializeData(FakeTileConfiguration configuration)
    {
        _targetPosition = configuration.TargetPosition;
        _speed = _distance / _time;
        _isStart = true;
    }

    private void Update()
    {
        if (_isStart)
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed);

            if (_distance<=0.1f)
            {
                OnDestroyed?.Invoke(this);
                gameObject.SetActive(false);
            }  
        }
    }
}
