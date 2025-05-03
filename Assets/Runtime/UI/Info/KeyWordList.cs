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

    private int extraCount;

    public void Initialize()
    {
        searcher = new();
        for (int i = 0; i < keywords.Count; i++)
        {
            searcher.Add(keywords[i].word, keywords[i].description);
        }
    }

    public string GetDescription(string word)
    {
        if (searcher.ContainsKey(word))
            return searcher[word];
        return string.Empty;
    }

    /// <summary>
    /// 找出一段文本中的所有关键词，对其添加前后缀，返回处理后的文本
    /// </summary>
    public string MarkAllKeyWords(string text, Func<string, string> Mark)
    {
        words = new string[keywords.Count];
        for (int i = 0; i < keywords.Count; i++)
        {
            words[i] = keywords[i].word;
        }
        string pattern = $"({string.Join("|", words)})";
        string ret = Regex.Replace(text, pattern, match =>
        {
            return Mark(match.Groups[1].Value);
        }, RegexOptions.IgnoreCase);
        return ret;
    }

    public void Push(string word, string description)
    {
        if (!searcher.ContainsKey(word))
        {
            keywords.Add(new KeyWord(word, description));
            searcher.Add(word, description);
            extraCount++;
        }
    }

    public void PopExtra()
    {
        for (; extraCount > 0; extraCount--)
        {
            string word = keywords[^1].word;
            keywords.RemoveAt(keywords.Count - 1);
            searcher.Remove(word);
        }
    }
}

[Serializable]
public class KeyWord
{
    public string word;
    [TextArea(3, 10)]
    public string description;

    public KeyWord(string word, string description)
    {
        this.word = word;
        this.description = description;
    }
}