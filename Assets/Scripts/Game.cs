using UnityEngine;
using System;

public class Game : MonoBehaviour
{
    public event Action<int> OnGameStart;
    public event Action OnExit;
    public event Action OnGameRestart;
    public event Action OnGameOver;
    
    [SerializeField] private Menu _menu;
    [SerializeField] private RestartScreen _restartScreen;
    [SerializeField] private InputController _inputController;
    [SerializeField] private Score _score;
    [SerializeField] private FieldController _fieldController;
 
    public bool InGame { get; private set; }

    private bool _isAlreadyWin=false;

    public void Awake()
    {
        if (_fieldController == null)
            throw new NullReferenceException();
        if (_restartScreen == null)
            throw new NullReferenceException();
        if (_inputController == null)
            throw new NullReferenceException();
        if (_menu == null)
            throw new NullReferenceException();

        InGame = false;
    }

    private void StartGame(int size)
    {
        OnGameStart?.Invoke(size);
        _isAlreadyWin = false;
        InGame = true;
    }

    private void Win()
    {
        if (!_isAlreadyWin)
        {
            _restartScreen.OnTryContiniePressed += TryContinue;
            _restartScreen.Win();
            _isAlreadyWin = true;
        }    
    }

    private void GameOver()
    {
        _restartScreen.OnTryContiniePressed += TryContinue;
        OnGameOver?.Invoke();
        _restartScreen.Lose();
    }

    private void TryContinue(bool isWint)
    {
        _restartScreen.OnTryContiniePressed -= TryContinue;
        if (!isWint)
            RestartGame();
    }

    private void RestartGame() => OnGameRestart?.Invoke();

    private void ExitToMenu()
    {
        OnExit?.Invoke();
        InGame = false;
    }

    private void HomeButtonPressed()
    {
        if (InGame)
            OnExit?.Invoke();
        Application.Quit();
    }

    private void EscapeButtonPressed()
    {
        if (InGame)
            ExitToMenu();
        else
            Application.Quit();
    }

    private void OnEnable()
    {
        _menu.OnPlayButtonPressed += StartGame;
        _menu.OnMenuButtonPressed += ExitToMenu;
        _menu.OnRestartButtonPressed += RestartGame;
        _inputController.OnEscapeButtonPressed += EscapeButtonPressed;
        _inputController.OnHomeButtonPressed += HomeButtonPressed;
        _fieldController.OnWin += Win;
        _fieldController.OnLose += GameOver;
    }

    private void OnDisable()
    {
        _menu.OnPlayButtonPressed -= StartGame;
        _menu.OnMenuButtonPressed -= ExitToMenu;
        _menu.OnRestartButtonPressed -= RestartGame;
        _inputController.OnEscapeButtonPressed -= EscapeButtonPressed;
        _inputController.OnHomeButtonPressed -= HomeButtonPressed;
        _fieldController.OnWin -= Win;
        _fieldController.OnLose -= GameOver;
    }
}

