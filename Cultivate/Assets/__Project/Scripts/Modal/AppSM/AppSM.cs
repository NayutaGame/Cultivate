
using System.Collections.Generic;
using System.Threading.Tasks;

public class AppSM
{
    private List<AppS> _stack;

    public AppS Current => Get(1);

    public AppS Get(int i)
    {
        if (_stack.Count >= i)
            return _stack[^i];
        return null;
    }

    public AppSM()
    {
        _stack = new List<AppS>();
    }

    public async Task Push(AppS state)
    {
        NavigateDetails d = new NavigateDetails(Current, state);
        if (Current != null)
            await Current.CEnter(d);
        _stack.Add(state);
        await Current.Enter(d);
    }

    public async Task Pop(int times = 1)
    {
        for (int i = 0; i < times; i++)
        {
            NavigateDetails d = new NavigateDetails(Current, Get(2));
            await Current.Exit(d);
            _stack.RemoveAt(_stack.Count - 1);
            if (Current != null)
                await Current.CExit(d);
        }
    }
}
