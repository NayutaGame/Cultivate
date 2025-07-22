
using System.Text;
using System.Text.RegularExpressions;

/**
    SkillDefinition
        => ProcedureDefinitions
        => GetDescription()
            => ProcessEnvironment
            => ApplyReplaceValues
            => ApplyResult
        => ProcessReturn
        => Highlight => Highlighted
        
    BuffView
        => RawDescription
        => Highlight => Highlighted
 */
public class Description
{
    private static readonly Regex ReplacingWhatsInBrackets = new(@"\[(.*?)\]", RegexOptions.Compiled);
    
    private StringBuilder _sb;
    public StringBuilder Sb => _sb;

    public Description()
    {
        _sb = new();
    }

    public Description(string description)
    {
        _sb = new(description);
    }

    public void Join(Description description)
    {
        _sb.Append(description.Sb);
    }

    public void AppendReturn()
    {
        _sb.Append("\n");
    }

    public void AppendSoftReturn()
    {
        _sb.Append("||");
    }

    public string GetHighlight(AnnotationArray cascade)
    {
        StringBuilder sb = new(_sb.ToString());
        foreach (LegacyAnnotatable annotatable in cascade.GetArray())
        {
            string name = annotatable.GetName();
            string replacement = $"<link=\"{name}\"><style=\"Highlight\">{name}</style></link>";
            sb = sb.Replace(name, replacement);
        }
        return sb.ToString();
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
            _sb.Clear();
            return;
        }
        
        string content = _sb.ToString();
        _sb.Clear();
        _sb.Append($"<style=\"{style}\">{content}</style>");
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
            _sb.Clear();
            return;
        }
        
        string content = _sb.ToString();
        _sb.Clear();
        _sb.Append($"<style=\"{style}\">{content}</style>");
    }

    public void ApplyReplaceValues(ResultDict castResult)
    {
        if (castResult == null) return;
    
        string content = _sb.ToString();
    
        string result = ReplacingWhatsInBrackets.Replace(content, match =>
        {
            string key = match.Groups[1].Value;
            return castResult.ContainsKey(key) ? castResult[key] : match.Value;
        });
    
        // 更新StringBuilder的内容
        _sb.Clear();
        _sb.Append(result);
    }

    public override string ToString()
        => _sb.ToString();

    public Description Clone()
        => new(_sb.ToString());
    
    public static implicit operator Description(string description) => new(description);
}