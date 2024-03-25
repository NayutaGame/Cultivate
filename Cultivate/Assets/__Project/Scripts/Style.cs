
using System.Collections.Generic;

public static class Style
{
    public static string ApplyStyle(this string s, CastResult castResult, string styleName)
    {
        if (castResult == null)
            return s;

        if (!castResult.ContainsKey(styleName))
            return s;
        
        string style = castResult[styleName];
        if (string.IsNullOrEmpty(style))
            return s;
        
        return $"<style=\"{style}\">{s}</style>";
    }

    public static string ApplyCond(this string s, CastResult castResult)
        => s.ApplyStyle(castResult, "cond");

    public static string ApplyOdd(this string s, CastResult castResult)
        => s.ApplyStyle(castResult, "odd");

    public static string ApplyEven(this string s, CastResult castResult)
        => s.ApplyStyle(castResult, "even");

    public static CastResult New() => new();

    public static CastResult Append(this CastResult castResult, string key, bool cond)
    {
        if (!cond)
            castResult[key] = "Grey";
        return castResult;
    }

    public static CastResult Merge(this CastResult castResult,
        CastResult other)
    {
        foreach (KeyValuePair<string, string> kvp in other)
            castResult[kvp.Key] = kvp.Value;

        return castResult;
    }

    public static CastResult CastResultFromBools(params bool[] bools)
    {
        var executeResult = New();
        for (int i = 0; i < bools.Length; i++)
            executeResult.Append(i.ToString(), bools[i]);
        return executeResult;
    }

    public static CastResult ToCastResult(this bool cond) => New().Append("cond", cond);

    public static CastResult CastResultFromOddEven(bool odd, bool even) =>
        New().Append("odd", odd).Append("even", even);
}
