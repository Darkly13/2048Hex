using UnityEngine;
using TMPro;
using System;

public class HighScore : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private Score _score;
    [SerializeField] private TextMeshProUGUI _highScoreText;

    
    private int _size;
    private int _value;

    public void Awake() 
    {
        if (_game == null)
            throw new NullReferenceException();
        if (_score == null)
            throw new NullReferenceException();
        if (_highScoreText == null)
            throw new NullReferenceException();
    }

    private void GameStart(int size)
    {
        _size = size;
        TryLoadHighScore(size);
    }

    private void GameRestart() => TrySaveHighScore();
    private void ExitToMenu() => TrySaveHighScore();
    private void UpdateHighScoretext() => _highScoreText.text = _value.ToString();

    private void TryLoadHighScore(int size)
    {
        string key = size.ToString();
        if (PlayerPrefs.HasKey(key))
            _value = PlayerPrefs.GetInt(key);
        else
            _value = 0;

        UpdateHighScoretext();
    }

    private void TrySaveHighScore()
    {
        PlayerPrefs.SetInt(_size.ToString(), _value);
    }

    private void ScoreValueChanged(int score)
    {
        if (_value < score)
        {
            _value = score;
            UpdateHighScoretext();
        }       
    }

    private void OnEnable()
    {     
        _game.OnGameStart += GameStart;
        _game.OnGameRestart += GameRestart;
        _game.OnGameOver += ExitToMenu;
        _game.OnExit += ExitToMenu;
        _score.OnValueChanged += ScoreValueChanged;
    }

    private void OnDisable()
    {            
        _game.OnGameStart -= GameStart;
        _game.OnGameRestart -= GameRestart;
        _game.OnGameOver -= ExitToMenu;
        _game.OnExit -= ExitToMenu;
        _score.OnValueChanged -= ScoreValueChanged;
    }
}
