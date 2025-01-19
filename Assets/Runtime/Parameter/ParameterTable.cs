using MyTool;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ParameterTable : ScriptableObject
{
    private Dictionary<string, Parameter> searcher;
    [SerializeField]
    private List<Parameter> parameters;
    [SerializeField]
    private List<string> resetParameters;

    public int GetValuePerUnit(string name)
    {
        int i = parameters.FindIndex(x => x.name == name);
        if (i == -1)
            return 0;
        return parameters[i].valuePerUnit;
    }

    public List<string> GetParameterNames()
    {
        List<string> ret = new();
        for (int i = 0; i < parameters.Count; i++)
        {
            ret.Add(parameters[i].name);
        }
        return ret;
    }

    public HashSet<string> GetResetParameters()
    {
        return resetParameters.ToHashSet();
    }
}

[System.Serializable]
public class Parameter
{
    public string name;
    public int valuePerUnit;
    public bool hidden;
    public string description;
}