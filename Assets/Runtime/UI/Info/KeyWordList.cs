using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu]
public class KeyWordList : ScriptableObject
{
    [SerializeField]
    private List<KeyWord> keywords;
    private Dictionary<string, string> searcher;
    private string[] words;

    public void Initialize()
    {
        searcher = new();
        words = new string[keywords.Count];
        for(int i = 0; i < keywords.Count; i++)
        {
            searcher.Add(keywords[i].word, keywords[i].description);
            words[i] = keywords[i].word;
        }
    }

    public string GetDescription(string word)
    {
        if(searcher.ContainsKey(word))
            return searcher[word];
        return string.Empty;
    }

    /// <summary>
    /// 找出一段文本中的所有关键词，对其添加前后缀，返回处理后的文本
    /// </summary>
    public string MarkAllKeyWords(string text, Func<string, string> Mark)
    {
        string pattern = $"({string.Join("|", words)})";
        string ret = Regex.Replace(text, pattern, match =>
        {
            return Mark(match.Groups[1].Value);
        }, RegexOptions.IgnoreCase);
        return ret;
    }
}

[Serializable]
public class KeyWord
{
    public string word;
    [TextArea(3,10)]
    public string description;
}