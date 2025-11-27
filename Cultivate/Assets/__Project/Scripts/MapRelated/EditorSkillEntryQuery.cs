
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class EditorSkillEntryQuery
{
    [SerializeField] public string EntryName;
    [SerializeField] public EditorWuXing WuXing;
    [SerializeField] public EditorJingJie LowBaseJingJie;
    [SerializeField] public EditorJingJie HighBaseJingJie;
    [SerializeField] public EditorTag Tag;
}