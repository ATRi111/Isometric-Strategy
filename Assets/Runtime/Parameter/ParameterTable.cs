using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class ParameterTable : ScriptableObject
{
    private readonly Dictionary<string, Parameter> searcher = new();

    [SerializeField]
    private List<Parameter> parameters;

    /// <summary>
    /// 自动重置参数
    /// </summary>
    [SerializeField]
    private List<string> resetParameters;

    public Parameter this[string parameterName] => searcher[parameterName];

    public int Count => parameters.Count;
    public string IndexToName(int index) => parameters[index].name;

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
    public void Initialize()
    {
        searcher.Clear();
        for (int i = 0; i < parameters.Count; i++)
        {
            searcher.Add(parameters[i].name, parameters[i]);
        }
    }
}

[System.Serializable]
public class Parameter
{
    public string name;
    public int maxValue = 10;
    public int valuePerUnit;
    public bool hidden;
    public string description;
}