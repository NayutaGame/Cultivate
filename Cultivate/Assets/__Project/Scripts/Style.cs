
using System.Collections.Generic;

public static class Style
{
    public static string ApplyStyle(this string s, Dictionary<string, string> indicator, string styleName)
    {
        if (indicator == null)
            return s;

        string style = indicator[styleName];
        if (string.IsNullOrEmpty(style))
            return s;
        
        return $"<style=\"{styleName}\">{s}</style>";
    }

    public static string ApplyCond(this string s, Dictionary<string, string> indicator) =>
        s.ApplyStyle(indicator, "cond");

    public static string ApplyOdd(this string s, Dictionary<string, string> indicator) =>
        s.ApplyStyle(indicator, "odd");

    public static string ApplyEven(this string s, Dictionary<string, string> indicator) =>
        s.ApplyStyle(indicator, "even");

    public static Dictionary<string, string> New() => new();

    public static Dictionary<string, string> Append(this Dictionary<string, string> indicator, string key, bool cond)
    {
        if (!cond)
            indicator[key] = "grey";
        return indicator;
    }

    public static Dictionary<string, string> Merge(this Dictionary<string, string> indicator,
        Dictionary<string, string> other)
    {
        foreach (KeyValuePair<string, string> kvp in other)
            indicator[kvp.Key] = kvp.Value;

        return indicator;
    }

    public static Dictionary<string, string> IndicatorFromBools(params bool[] bools)
    {
        var indicator = New();
        for (int i = 0; i < bools.Length; i++)
            indicator.Append(i.ToString(), bools[i]);
        return indicator;
    }

    public static Dictionary<string, string> ToIndicator(this bool cond) => New().Append("cond", cond);

    public static Dictionary<string, string> IndicatorFromOddEven(bool odd, bool even) =>
        New().Append("odd", odd).Append("even", even);
}
