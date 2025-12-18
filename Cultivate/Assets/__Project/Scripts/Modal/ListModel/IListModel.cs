
using System;
using Cysharp.Threading.Tasks;

public interface IListModel
{
    event Func<int, object, UniTask> InsertEvent;
    event Func<int, UniTask> RemoveAtEvent;
    event Func<int, UniTask> ModifiedEvent;
    event Func<UniTask> ResyncEvent;
    int Count();
    object Get(int index);
}

public interface IListModel<out T> where T : ISearchable
{
    void SetSearchText(string text);
}