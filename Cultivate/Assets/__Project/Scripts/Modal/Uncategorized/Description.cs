
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CLLibrary;
using UnityEngine;

public class Description
{
    private static readonly Regex ReplacingWhatsInBrackets = new(@"\[(.*?)\]", RegexOptions.Compiled);
    private static readonly Regex SplitterRegex = new Regex(@"(?:\n|\|\|)+", RegexOptions.Compiled);
    private static readonly Regex TrimReturnRegex = new Regex(@"^(?:\n|\|\|)+|(?:\n|\|\|)+$", RegexOptions.Compiled);
    private static Regex SymbolizeRegex;
    private static readonly Regex FindSymbolRegex = new Regex(@"\[(tag|buff|keyword|skill|character|jingJie):([^\]]+)\]", RegexOptions.Compiled);
    
    private Dirty<string> CalcHighlightedString;
    private StringBuilder _sb;

    public static void BuildSymbolizeRegex()
    {
        var patterns = new List<string>();
        
        foreach (TagEntry keyword in Encyclopedia.TagCategory)
            patterns.Add(Regex.Escape(keyword.GetName()));
        
        foreach (KeywordEntry keyword in Encyclopedia.KeywordCategory)
            patterns.Add(Regex.Escape(keyword.GetName()));
        
        foreach (BuffEntry buff in Encyclopedia.BuffCategory)
            patterns.Add(Regex.Escape(buff.GetName()));

        foreach (SkillEntry skill in Encyclopedia.SkillCategory)
            patterns.Add(Regex.Escape(skill.GetName()));

        foreach (CharacterEntry character in Encyclopedia.CharacterCategory)
            patterns.Add(Regex.Escape(character.GetName()));
        
        foreach (JingJie jingJie in JingJie.Traversal)
            patterns.Add(Regex.Escape(jingJie.GetName()));
        
        string pattern = string.Join("|", patterns);
        SymbolizeRegex = new Regex($@"(?<!\[)({pattern})(?!\])", RegexOptions.Compiled);
    }
    
    public Description()
    {
        _sb = new();
        CalcHighlightedString = new Dirty<string>(CalcHighlightedStringFunc);
    }

    public Description(string mixed)
    {
        string symbolized = Symbolize(mixed);
        _sb = new(symbolized);
        CalcHighlightedString = new Dirty<string>(CalcHighlightedStringFunc);
    }

    private string Symbolize(string mixed)
    {
        return SymbolizeRegex.Replace(mixed, match =>
        {
            string keyword = match.Value;
            
            if (Encyclopedia.TagCategory.ContainsKey(keyword))
                return $"[tag:{keyword}]";
            
            if (Encyclopedia.KeywordCategory.ContainsKey(keyword))
                return $"[keyword:{keyword}]";
            
            if (Encyclopedia.BuffCategory.ContainsKey(keyword))
                return $"[buff:{keyword}]";

            if (Encyclopedia.SkillCategory.ContainsName(keyword))
                return $"[skill:{keyword}]";
            
            if (Encyclopedia.CharacterCategory.ContainsKey(keyword))
                return $"[character:{keyword}]";
            
            if (JingJie.ContainsName(keyword))
                return $"[jingJie:{keyword}]";
            
            return keyword;
        });
    }

    private string Desymbolize(string symbolized)
    {
        return FindSymbolRegex.Replace(symbolized, match => match.Groups[2].Value);
    }

    public void Join(string mixed)
    {
        string symbolized = Symbolize(mixed);
        _sb.Append(symbolized);
        CalcHighlightedString.SetDirty();
    }

    public void Join(Description description)
    {
        _sb.Append(description.GetSymbolizedString());
        CalcHighlightedString.SetDirty();
    }

    public void Clear()
    {
        _sb.Clear();
        CalcHighlightedString.SetDirty();
    }

    public void AppendReturn()
    {
        _sb.Append("\n");
        CalcHighlightedString.SetDirty();
    }

    public void AppendSoftReturn()
    {
        _sb.Append("||");
        CalcHighlightedString.SetDirty();
    }

    public void ApplyResult(ResultDict castResult, string key)
        => ApplyStyle(castResult, key);
    
    public void ApplyStyle(ResultDict castResult, string styleKey)
    {
        if (castResult == null || styleKey == null)
            return;

        if (!castResult.ContainsKey(styleKey))
            return;
        
        string style = castResult[styleKey];
        if (string.IsNullOrEmpty(style))
            return;

        if (style == "Hide")
        {
            Clear();
            return;
        }
        
        string content = GetSymbolizedString();
        Clear();
        Join($"<style=\"{style}\">");
        Join(content);
        Join("</style>");
    }
    
    public void ApplyStyle(ResultDict castResult, object styleKey)
    {
        if (castResult == null || styleKey == null)
            return;

        if (!castResult.ContainsKey(styleKey))
            return;
        
        string style = castResult[styleKey];
        if (string.IsNullOrEmpty(style))
            return;

        if (style == "Hide")
        {
            Clear();
            return;
        }
        
        string content = GetSymbolizedString();
        Clear();
        Join($"<style=\"{style}\">");
        Join(content);
        Join("</style>");
    }

    public void ApplyReplaceValues(ResultDict castResult)
    {
        if (castResult == null) return;

        string content = GetSymbolizedString();
    
        string result = ReplacingWhatsInBrackets.Replace(content, match =>
        {
            string key = match.Groups[1].Value;
            return castResult.ContainsKey(key) ? castResult[key] : match.Value;
        });
    
        Clear();
        Join(result);
    }

    public static string ProcessExtraReturn(string symbolized)
    {
        string content = TrimReturnRegex.Replace(symbolized, "");

        string[] groups = SplitterRegex.Split(content);
        MatchCollection matchCollection = SplitterRegex.Matches(content);

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < groups.Length - 1; i++)
        {
            sb.Append(groups[i]);
            string split = matchCollection[i].Value;
            sb.Append(split.Contains("\n") ? "\n" : "||");
        }

        sb.Append(groups[^1]);

        return sb.ToString();
    }

    public static string ProcessSoftReturn(string symbolized, int maxConvert = 2)
    {
        const string SOFT_RETURN = "||";
        const string HARD_RETURN = "\n";
        const string SPACE = "  ";

        string content = symbolized;
        int hardReturnCount = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] == '\n')
                hardReturnCount++;
        }

        int convert = Mathf.Max(0, maxConvert - hardReturnCount);
        int replaced = 0;
        int idx = 0;
        StringBuilder sb = new StringBuilder();
        while (idx < content.Length)
        {
            int softIdx = content.IndexOf(SOFT_RETURN, idx);
            if (softIdx == -1)
            {
                sb.Append(content.Substring(idx));
                break;
            }
            sb.Append(content.Substring(idx, softIdx - idx));
            if (replaced < convert)
            {
                sb.Append(HARD_RETURN);
                replaced++;
            }
            else
            {
                sb.Append(SPACE);
            }
            idx = softIdx + SOFT_RETURN.Length;
        }

        return sb.ToString();
    }
    
    public static string ProcessHighlight(string symbolized)
    {
        return FindSymbolRegex.Replace(symbolized, match =>
        {
            string name = match.Groups[2].Value;
            return $"<link=\"{name}\"><style=\"Highlight\">{name}</style></link>";
        });
    }

    public string GetRawString()
    {
        // "灵气+1"
        return Desymbolize(_sb.ToString());
    }

    public string GetSymbolizedString()
    {
        // "[buff:灵气]+1"
        return _sb.ToString();
    }

    public string GetHighlightedString()
    {
        // "<link="灵气"><style="Highlight">灵气</style></link>+1"
        return CalcHighlightedString.Value;
    }

    private string CalcHighlightedStringFunc()
    {
        string symbolized = GetSymbolizedString();
        symbolized = ProcessExtraReturn(symbolized);
        symbolized = ProcessSoftReturn(symbolized);
        symbolized = ProcessHighlight(symbolized);
        return symbolized;
    }

    public Description Clone()
        => new(GetSymbolizedString());
    
    public static implicit operator Description(string mixed) => new(mixed);
}