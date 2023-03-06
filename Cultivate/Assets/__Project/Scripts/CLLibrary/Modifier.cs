
using System.Collections.Generic;

namespace CLLibrary
{
    public class Modifier : Dirty<ModifierLeaf>
    {
        private List<Modifier> _parents;
        private List<Modifier> _children;
        private List<ModifierLeaf> _leaves;

        private Modifier() : base(() => new ModifierLeaf())
        {
            _parents = new List<Modifier>();
            _children = new List<Modifier>();
            _leaves = new List<ModifierLeaf>();
        }

        public static Modifier Default => new Modifier();

        public override ModifierLeaf Value
        {
            get
            {
                if (!_dirty) return _value;

                _value = _generator();
                _dirty = false;

                foreach (Modifier c in _children) MergeDict(_value, c.Value);
                foreach (ModifierLeaf l in _leaves) MergeDict(_value, l);

                return _value;
            }
        }

        public override void SetDirty()
        {
            base.SetDirty();
            foreach (var p in _parents) p.SetDirty();
        }

        public void AddParent(Modifier parent)
        {
            _parents.Add(parent);
            parent._children.Add(this);
            SetDirty();
        }

        public void AddChild(Modifier child)
        {
            child.AddParent(this);
        }

        public void RemoveParent(Modifier parent)
        {
            SetDirty();
            _parents.Remove(parent);
            parent._children.Remove(this);
        }

        public void RemoveChild(Modifier child)
        {
            child.RemoveParent(this);
        }

        public void AddLeaf(ModifierLeaf leaf)
        {
            _leaves.Add(leaf);
            leaf.Changed += SetDirty;
            SetDirty();
        }

        public void RemoveLeaf(ModifierLeaf leaf)
        {
            leaf.Changed -= SetDirty;
            _leaves.Remove(leaf);
            SetDirty();
        }

        public void ClearLeaves()
        {
            _leaves.Clear();
            SetDirty();
        }

        private static void MergeDict(ModifierLeaf thisDict, ModifierLeaf otherDict)
        {
            foreach (var kvp in otherDict)
            {
                if (thisDict.ContainsKey(kvp.Key))
                {
                    thisDict[kvp.Key] += kvp.Value;
                }
                else
                {
                    thisDict[kvp.Key] = kvp.Value;
                }
            }
        }

        public ModifierSnapshot GetSnapshot() => ModifierSnapshot.FromDictionary(Value);

        public override string ToString()
        {
            return $"modifier child count = {_children.Count}, leaves count = {_leaves.Count}";
        }
    }
}
