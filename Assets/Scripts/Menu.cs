using UnityEngine;
using System;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public event Action<int> OnPlayButtonPressed;
    public event Action OnMenuButtonPressed;
    public event Action OnRestartButtonPressed;

    [SerializeField] private Toggle _defaultToggle;

    private int _selectedSize;

    public void Awake()
    {
        _defaultToggle.isOn = true;
        _defaultToggle.Select();
    }

    public void SelectSize(int size)
    {
        if (size < 3)
            throw new ArgumentOutOfRangeException();

        _selectedSize = size;
    }

    public void Play()
    {
        OnPlayButtonPressed?.Invoke(_selectedSize);
        gameObject.SetActive(false);
    }

    public void GoToMenu()
    {
        gameObject.SetActive(true);
        OnMenuButtonPressed?.Invoke();
    }

    public void Restart() => OnRestartButtonPressed?.Invoke();
}
