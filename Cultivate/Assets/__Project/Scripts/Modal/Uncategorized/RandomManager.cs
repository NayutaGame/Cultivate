using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class RandomManager : Singleton<RandomManager>
{
    public static float value
    {
        get
        {
            return Random.value;
        }
    }

    public static float Range(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static int Range(int min, int max)
    {
        return Random.Range(min, max);
    }
}
