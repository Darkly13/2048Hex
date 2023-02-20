using System.Collections.Generic;

public class FieldSave
{
    public HexMassive Field { get; private set; }
    public List<string> ValuesList;
    public int Score;

    private List<int> _values;
    public int Size { get; private set; }


    public FieldSave()
    {
        ValuesList = new List<string>();
        _values = new List<int>();
    }

    public void SetSize(int size)
    {
        Size = size;
    }

    public void SetValues(HexMassive field, int score)
    {
        Field = field;
        Score = score;
    }

    public void SerializeData()
    {
        Tile[][] massive = Field.Massive;
        foreach(var line in massive)
        {
            foreach(var tile in line)
            {
                ValuesList.Add(tile.Value.ToString());
            }
        }        
    }

    public void DeserializeField()
    {
        int index = 0;
        foreach(var str in ValuesList)
        {
            _values.Add(int.Parse(str));
            index++;
        }
    }

    public List<int> GetValues()
    {
        return _values;
    }
}
