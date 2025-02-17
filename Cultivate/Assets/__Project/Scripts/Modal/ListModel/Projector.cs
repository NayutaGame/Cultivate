
using System;
using Cysharp.Threading.Tasks;

public class Projector<T> : IListModel
{
    private IListModel _source;
    private Func<object, T> _projection;
    
    public Projector(IListModel source, Func<object, T> projection)
    {
        _source = source;
        _projection = projection;
    }
    
    public int Count() => _source.Count();
    public object Get(int index) => _projection(_source.Get(index));

    public event Func<int, object, UniTask> InsertEvent;
    public event Func<int, UniTask> RemoveAtEvent;
    public event Func<int, UniTask> ModifiedEvent;
    public event Func<UniTask> ResyncEvent;
}
