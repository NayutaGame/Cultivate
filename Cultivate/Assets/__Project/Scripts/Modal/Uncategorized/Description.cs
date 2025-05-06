
using System.Text;
using System.Text.RegularExpressions;

public class Description
{
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

    public string GetHighlight(AnnotationArray cascade)
    {
        StringBuilder sb = new(_sb.ToString());
        foreach (Annotatable annotatable in cascade.GetArray())
            sb = sb.Replace(annotatable.GetName(), $"<style=\"Highlight\">{annotatable.GetName()}</style>");

        return sb.ToString();
    }

    public void ApplyCastResult(ResultDict castResult, string key)
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
        // 匹配形如 [Key] 的模式
        string pattern = @"\[(.*?)\]";
    
        // 使用正则表达式替换所有匹配项
        string result = Regex.Replace(content, pattern, match =>
        {
            string key = match.Groups[1].Value; // 获取括号中的key
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