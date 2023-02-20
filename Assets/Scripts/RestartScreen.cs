using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RestartScreen : MonoBehaviour
{
    public event Action<bool> OnTryContiniePressed;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _winBackgroundColor;
    [SerializeField] private Color _winTextColor;
    [SerializeField] private Color _loseBackgroundColor;
    [SerializeField] private Color _loseTextColor;

    private const string LOSE_TEXT = "GAME OVER\nTAP TO RESTART";
    private const string WIN_TEXT = "YOU WIN\nTAP TO CONTINUE";

    private bool _isWin;

    public void Awake()
    {
        if (_image == null)
            throw new NullReferenceException();

        if (_text == null)
            throw new NullReferenceException();
    }

    public void Win()
    {
        gameObject.SetActive(true);
        _image.color = _winBackgroundColor;
        _text.color = _winTextColor;
        _text.text = WIN_TEXT;
        _isWin = true;
    }

    public void Lose()
    {
        gameObject.SetActive(true);
        _image.color = _loseBackgroundColor;
        _text.color = _loseTextColor;
        _text.text = LOSE_TEXT;
        _isWin = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTryContiniePressed?.Invoke(_isWin);
            gameObject.SetActive(false);
        }
    }
}
