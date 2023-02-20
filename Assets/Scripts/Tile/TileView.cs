using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    [SerializeField] private Tile _model;
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private Image _image;
    [SerializeField] private Animator _animator;

    private const string APPEAR_ANIMATION = "Appear";
    private const string MULTIPLY_ANIMATION = "Multiply";
 
    private int _value;  
    private bool _isMultiply=false;
    private bool _isMoving=false;
    private Vector2 _sizeDelta;


    private void Start() => _sizeDelta = GetComponent<RectTransform>().sizeDelta;
    private void Created() => _animator.Play(APPEAR_ANIMATION);
    private void Multiplied(int value) => _isMultiply = true;

    private void ValueChanged(int value)
    {
        _value = value;
        if (!_isMoving)
            UpdateView();     
    }

    private void Moved(Tile from)
    {
        _isMoving = true;
        FakeTile fakeTile = FakeTilesPool.instant.GetFreeElement();
        FakeTileConfiguration configuration = new FakeTileConfiguration(from.Value, from.transform.position, transform.position, _sizeDelta);
        fakeTile.Initialize(configuration);
        fakeTile.gameObject.SetActive(true);
        fakeTile.OnDestroyed += FakeTileDestroyed;
    }

    private void FakeTileDestroyed(FakeTile fakeTile)
    {
        fakeTile.OnDestroyed -= FakeTileDestroyed;
        _isMoving = false;
        UpdateView();
        if (_isMultiply)
        {
            _isMoving = false;
            _isMultiply = false;
            _animator.Play(MULTIPLY_ANIMATION);
        }            
    }

    private void UpdateView()
    {
        _image.color = Colors.instant.GetColor(_value);
        _valueText.text = _value == 0 ? "" : Mathf.Pow(2, _value).ToString();
        _valueText.color = Colors.instant.GetTextColor(_value);
    }

    private void OnEnable()
    {
        _model.OnCreated += Created;
        _model.OnValueChanged += ValueChanged;
        _model.OnMultiplied += Multiplied;
        _model.OnMoved += Moved;
    }

    private void OnDisable()
    {
        _model.OnCreated -= Created;
        _model.OnValueChanged -= ValueChanged;
        _model.OnMultiplied -= Multiplied;
        _model.OnMoved -= Moved;
    }
}
