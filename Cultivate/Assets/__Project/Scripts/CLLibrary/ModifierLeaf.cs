
using System;
using System.Collections.Generic;

namespace CLLibrary
{
    public class ModifierLeaf : Dictionary<string, float>
    {
        public ModifierLeaf Clone()
        {
            ModifierLeaf l = new ModifierLeaf();
            foreach (var kvp in this) l[kvp.Key] = kvp.Value;
            return l;
        }

        public float ForceGet(string key)
        {
            if (ContainsKey(key)) return this[key];
            this[key] = 0;
            Changed?.Invoke();
            return this[key];
        }

        public void ForceSet(string key, float value)
        {
            this[key] = value;
            Changed?.Invoke();
        }

        public void ForceAdd(string key, float value)
        {
            if (ContainsKey(key))
            {
                this[key] += value;
            }
            else
            {
                this[key] = value;
            }
            Changed?.Invoke();
        }

        public event Action Changed;
    }
}
