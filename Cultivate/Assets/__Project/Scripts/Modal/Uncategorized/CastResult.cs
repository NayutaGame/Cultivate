
using System.Collections.Generic;

public class CastResult : Dictionary<string, string>
{
    private static CastResult _default;

    public static CastResult Default
    {
        get
        {
            if (_default != null)
                return _default;

            _default = new();
            _default["doubleEnd"] = "Hide";
            return _default;
        }
    }
}
