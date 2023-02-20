using UnityEngine;
using System;

public class Tile : MonoBehaviour
{
    public event Action OnCreated;
    public event Action<int> OnValueChanged;
    public event Action<int> OnMultiplied;
    public event Action<Tile> OnMoved;
    public event Action OnWin;

    public int X { get; private set; }
    public int Y { get; private set; }
    public int Value { get; private set; }
    public bool IsEmpty => Value == 0;
    public bool WasMultiplied { get; private set; }

    private Game _game;

    public void Clear() => SetValue(0);
    public bool CanMultiplyWith(Tile other) => Value == other.Value;                   //Пока никем не вызывается
    public void ClearMultiply() => WasMultiplied = false;
    private void MoveFinished() => ClearMultiply();


    public void SetXY(int x, int y)
    {
        X = x;
        Y = y;
        WasMultiplied = false;
    }

    public void SetValue(int value)
    {
        if (value < 0)
            throw new IndexOutOfRangeException();

        Value = value;
        OnValueChanged?.Invoke(value);
    }

    public void Create(int value)
    {
        OnCreated?.Invoke();
        SetValue(value);      
    }

    public void MultiplyWith(Tile other)
    {
        OnMultiplied?.Invoke((int)Mathf.Pow(2, Value));
        OnMoved?.Invoke(other);
        SetValue(Value+1);
        other.Clear();      
        WasMultiplied = true;

        if (Value == 11)
             OnWin?.Invoke();
    }

    public void SetInEmpty(Tile other)
    {
        OnMoved?.Invoke(other);
        SetValue(other.Value);
        other.Clear();     
    }
}
