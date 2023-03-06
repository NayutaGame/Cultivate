
using System.Collections.Generic;

namespace CLLibrary
{
    public class ModifierSnapshot
    {
        public Dictionary<string, float> _value;

        private ModifierSnapshot(Dictionary<string, float> value)
        {
            if (value != null)
            {
                _value = new Dictionary<string, float>(value);
            }
            else
            {
                _value = new Dictionary<string, float>();
            }
        }

        public static ModifierSnapshot Default => new ModifierSnapshot(null);

        public static ModifierSnapshot FromDictionary(Dictionary<string, float> dictionary) =>
            new ModifierSnapshot(dictionary);
    }
}
