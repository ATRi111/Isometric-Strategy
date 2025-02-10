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
    /// �ҳ�һ���ı��е����йؼ��ʣ��������ǰ��׺�����ش������ı�
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