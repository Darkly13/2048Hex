using UnityEngine;
using System.IO;
using System;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private FieldController _fieldController;
    [SerializeField] private Score _score;

    private FieldSave _save;

    private void Exit() => MakeNewSave(_fieldController.Tiles.Size);
    private void GameRestart() => MakeNewSave(_fieldController.Tiles.Size);

    private void Awake()
    {
        if (_fieldController == null)
            throw new NullReferenceException();
        if (_score == null)
            throw new NullReferenceException();
    }

    private void GameFieldCreated(HexMassive massive)
    {
        if (TryLoad(massive.Size))
        {
            _fieldController.Tiles.InitializeLoadedValues(_save.GetValues());
            _score.AddScore(_save.Score);
        }
        else
        {
            MakeNewSave(massive.Size);
            _score.Clear();
        }
    }

    private bool TryLoad(int size)
    {
        string fileName = "FieldSave_" + size + ".json";
        string path = GetPath(fileName);

        if (File.Exists(path))
        {
            _save = JsonUtility.FromJson<FieldSave>(File.ReadAllText(path));
            _save.DeserializeField();
            return true;
        }

        return false;
    }

    private void MakeNewSave(int size)
    {
        FieldSave save = new FieldSave();
        save.SetSize(size);
        save.SetValues(_fieldController.Tiles, _score.Value);
        Save(save);
    }

    private void Save(FieldSave save)
    {
        save.SerializeData();
        string fileName = "FieldSave_" + save.Size + ".json";
        string path = GetPath(fileName);
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }

    private string GetPath(string fileName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.persistentDataPath+"Saves", fileName);
#else
        string path = Path.Combine(Application.dataPath + "Saves", fileName);
#endif
        return path;
    }

    private void OnEnable()
    {
        _fieldController.OnGameFieldCreated += GameFieldCreated;
        _game.OnExit += Exit;
        _game.OnGameRestart += GameRestart;
    }

    private void OnDisable()
    {
        _fieldController.OnGameFieldCreated -= GameFieldCreated;
        _game.OnExit -= Exit;
        _game.OnGameRestart -= GameRestart;
    }
}
