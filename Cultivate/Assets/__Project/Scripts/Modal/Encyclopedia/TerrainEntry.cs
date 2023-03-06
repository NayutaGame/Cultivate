using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEntry : Entry
{
    private string _description;
    public string Description;

    public int XiuWei;
    public int ChanNeng;

    public TerrainEntry(string name, string description, int xiuWei = 0, int chanNeng = 0) : base(name)
    {
        _description = description;
        XiuWei = xiuWei;
        ChanNeng = chanNeng;
    }
}
