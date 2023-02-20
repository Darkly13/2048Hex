using System.Collections.Generic;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public static Colors instant = null;

    [SerializeField] private List<Color> _colors;
    [SerializeField] private Color _darkText;
    [SerializeField] private Color _lightText;

    public void Awake()
    {
        if (instant == null)
            instant = this;

        else if (instant == this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public Color GetColor(int index)
    {
        if (index > 12)
            index = 12;
        return _colors[index];
    }

    public Color GetTextColor(int value) => value > 2 ? _lightText : _darkText;
}
