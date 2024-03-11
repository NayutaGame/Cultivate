
using System.Collections.Generic;

public static class Style
{
    public static string ApplyStyle(this string s, ExecuteResult executeResult, string styleName)
    {
        if (executeResult == null)
            return s;

        if (!executeResult.ContainsKey(styleName))
            return s;
        
        string style = executeResult[styleName];
        if (string.IsNullOrEmpty(style))
            return s;
        
        return $"<style=\"{style}\">{s}</style>";
    }

    public static string ApplyCond(this string s, ExecuteResult executeResult) =>
        s.ApplyStyle(executeResult, "cond");

    public static string ApplyOdd(this string s, ExecuteResult executeResult) =>
        s.ApplyStyle(executeResult, "odd");

    public static string ApplyEven(this string s, ExecuteResult executeResult) =>
        s.ApplyStyle(executeResult, "even");

    public static ExecuteResult New() => new();

    public static ExecuteResult Append(this ExecuteResult executeResult, string key, bool cond)
    {
        if (!cond)
            executeResult[key] = "Grey";
        return executeResult;
    }

    public static ExecuteResult Merge(this ExecuteResult executeResult,
        ExecuteResult other)
    {
        foreach (KeyValuePair<string, string> kvp in other)
            executeResult[kvp.Key] = kvp.Value;

        return executeResult;
    }

    public static ExecuteResult ExecuteResultFromBools(params bool[] bools)
    {
        var executeResult = New();
        for (int i = 0; i < bools.Length; i++)
            executeResult.Append(i.ToString(), bools[i]);
        return executeResult;
    }

    public static ExecuteResult ToExecuteResult(this bool cond) => New().Append("cond", cond);

    public static ExecuteResult ExecuteResultFromOddEven(bool odd, bool even) =>
        New().Append("odd", odd).Append("even", even);
}
