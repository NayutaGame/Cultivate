using System;
using UnityEngine;

[Serializable]
public class EditorEntityQuery
{
    [SerializeField] public string EntryName = "";
    [SerializeField] public int Ladder = -1; // -1 表示未指定
    [SerializeField] public int TargetDifficulty = -1; // -1 表示未指定
    [SerializeField] public bool LimitToPool = true;
}
