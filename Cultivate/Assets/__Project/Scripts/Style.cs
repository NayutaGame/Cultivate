
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
    
    public static string ApplyAttack(this string s)
        => $"<style=\"Attack\">{s}</style>";
    
    public static string ApplyDefend(this string s)
        => $"<style=\"Defend\">{s}</style>";
    
    public static string ApplyMana(this string s)
        => $"<style=\"Mana\">{s}</style>";
    
    public static string ApplyHeal(this string s)
        => $"<style=\"Heal\">{s}</style>";
    
    public static string ApplyDebuff(this string s)
        => $"<style=\"Debuff\">{s}</style>";
    
    public static string ApplyCond(this string s, CastResult castResult)
        => s.ApplyStyle(castResult, "cond");


    public static void Append(this CastResult castResult, string key, bool cond)
    {
        if (!cond)
        {
            castResult[key] = "Inactive";
            return;
        }
        
        if (castResult.ContainsKey(key))
            castResult.Remove(key);
    }

    public static void AppendCond(this CastResult castResult, bool cond)
        => castResult.Append("cond", cond);

    public static void AppendBools(this CastResult castResult, params bool[] bools)
    {
        for (int i = 0; i < bools.Length; i++)
            castResult.Append(i.ToString(), bools[i]);
    }

    public static void AppendBool(this CastResult castResult, int n, bool value)
    {
        castResult.Append(n.ToString(), value);
    }
}
