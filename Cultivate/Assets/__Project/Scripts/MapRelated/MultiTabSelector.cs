
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class MultiTabSelector
{
    [SerializeField] private string[] _tabNames;
    [SerializeField] private int _mask;
    [SerializeField] private bool _allowMultiple;
    
    public MultiTabSelector(string[] names, bool multiple = true)
    {
        _tabNames = names ?? Array.Empty<string>();
        _mask = 0;
        _allowMultiple = multiple;
    }
    
    public int GetSelectedMask() => _mask;
    public string[] GetTabNames() => _tabNames;
    public bool AllowMultiple() => _allowMultiple;
    
    public bool IsSelected(int index)
    {
        if (index < 0 || index >= _tabNames.Length) return false;
        return (_mask & (1 << index)) != 0;
    }
    
    public void Toggle(int index)
    {
        if (index < 0 || index >= _tabNames.Length) return;
        
        if (_allowMultiple)
        {
            _mask ^= (1 << index);
        }
        else
        {
            _mask = (1 << index);
        }
    }
    
    public void SetSelected(int index, bool selected)
    {
        if (index < 0 || index >= _tabNames.Length) return;
        
        if (selected)
        {
            if (_allowMultiple)
            {
                _mask |= (1 << index);
            }
            else
            {
                _mask = (1 << index);
            }
        }
        else
        {
            _mask &= ~(1 << index);
        }
    }
    
    public void SetTabNames(string[] names)
    {
        _tabNames = names ?? Array.Empty<string>();
        int maxIndex = _tabNames.Length - 1;
        if (maxIndex >= 0)
        {
            int validMask = (1 << (maxIndex + 1)) - 1;
            _mask &= validMask;
        }
        else
        {
            _mask = 0;
        }
    }
    
    public void SetAllowMultiple(bool multiple)
    {
        _allowMultiple = multiple;
        if (!multiple && _mask != 0)
        {
            int firstSelected = 0;
            while (firstSelected < _tabNames.Length && !IsSelected(firstSelected))
            {
                firstSelected++;
            }
            _mask = firstSelected < _tabNames.Length ? (1 << firstSelected) : 0;
        }
    }
    
    public int GetSelectedCount()
    {
        int count = 0;
        for (int i = 0; i < _tabNames.Length; i++)
        {
            if (IsSelected(i)) count++;
        }
        return count;
    }
    
    public int[] GetSelectedIndices()
    {
        var indices = new List<int>();
        for (int i = 0; i < _tabNames.Length; i++)
        {
            if (IsSelected(i)) indices.Add(i);
        }
        return indices.ToArray();
    }
}
