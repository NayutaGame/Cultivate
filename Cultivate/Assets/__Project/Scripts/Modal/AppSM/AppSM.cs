using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using UnityEngine;

public class AppSM
{
    private List<AppS> _stack;

    public AppS Current
    {
        get
        {
            if (_stack.Count >= 1)
                return _stack[^1];
            return null;
        }
    }

    public AppS SecondCurrent
    {
        get
        {
            if (_stack.Count >= 2)
                return _stack[^2];
            return null;
        }
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

    public async Task Pop()
    {
        NavigateDetails d = new NavigateDetails(Current, SecondCurrent);
        await Current.Exit(d);
        _stack.RemoveAt(_stack.Count - 1);
        if (Current != null)
            await Current.CExit(d);
    }
}
