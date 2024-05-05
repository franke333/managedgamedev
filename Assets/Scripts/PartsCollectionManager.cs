using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsCollectionManager : PersistentSingletonClass<PartsCollectionManager>
{
    private List<PartSO> _parts;
    private Dictionary<string, int> _partMap;
    private Dictionary<char, int> _partIDMap;

    public PartController PartControllerPrefab { private set; get; }

    override protected void Awake()
    {
        base.Awake();
        LoadParts();
    }

    /// <summary>
    /// Load all parts from resources
    /// </summary>
    /// <returns>Count of loaded parts</returns>
    public int LoadParts()
    {
        //clean
        _parts = new List<PartSO>();
        _partMap = new Dictionary<string, int>();
        _partIDMap = new Dictionary<char, int>();

        //Load all parts from resources
        _parts = new List<PartSO>(Resources.LoadAll<PartSO>("Parts"));
        for (int i = 0; i < _parts.Count; i++)
        {
            if (_partMap.ContainsKey(_parts[i]._name))
                Debug.LogError("Duplicate part name: " + _parts[i]._name);
            _partMap.Add(_parts[i]._name, i);
            _partIDMap.Add(_parts[i].ID, i);
        }

        return _parts.Count;
    }

    public PartSO GetPart(int index)
    {
        return _parts[index];
    }

    public PartSO GetPart(string name)
    {
        return _parts[_partMap[name]];
    }

    public PartSO GetPart(char ID)
    {
        return _parts[_partIDMap[ID]];
    }

    public IReadOnlyList<PartSO> GetParts() => _parts;
}
