
using System;

public class PackDescriptor
{
    private Predicate<PackEntry> _pred;
    private PackEntry _entry;
    private WuXing _wuXing;

    public PackDescriptor(
        Predicate<PackEntry> pred = null,
        PackEntry entry = null,
        WuXing wuXing = null)
    {
        _pred = pred;
        _entry = entry;
        _wuXing = wuXing;
    }

    public static PackDescriptor FromEntry(PackEntry entry)
        => new(entry: entry);

    public static PackDescriptor FromWuXing(WuXing wuXing)
        => new(wuXing: wuXing);

    public static PackDescriptor AnyPack()
        => new();

    public bool Contains(PackEntry packEntry)
    {
        if (_entry != null && packEntry != _entry)
            return false;
        
        if (_pred != null && !_pred(packEntry))
            return false;

        if (_wuXing != null && packEntry.WuXing != _wuXing)
            return false;

        return true;
    }

    public bool Contains(PackDescriptor descriptor)
    {
        if (_entry != null && _entry != descriptor._entry)
            return false;

        return descriptor.Contains(_entry);
    }

    public static implicit operator PackDescriptor(PackEntry packEntry) => FromEntry(packEntry);

    public string GetName()
    {
        // 如果有Entry，则返回Entry的名称
        if (_entry != null)
            return _entry.GetName();

        // 如果有WuXing，则返回WuXing的名称
        if (_wuXing != null)
            return _wuXing.ToString();
        
        // 如果有Pred，则抛出异常
        if (_pred != null)
            throw new NotImplementedException();

        // 如果没有任何条件，则返回"任意"
        return "任意";
    }
}
