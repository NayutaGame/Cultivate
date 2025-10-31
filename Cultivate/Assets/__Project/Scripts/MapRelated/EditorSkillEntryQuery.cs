
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class EditorSkillEntryQuery
{
    [SerializeField] public string EntryName = "";
    [SerializeField] public EditorWuXing WuXing = EditorWuXing.无;
    [SerializeField] public Bound BaseJingJieBound = JingJie.LianQi2HuaShen;
    [SerializeField] public EditorTag Tag = EditorTag.无;
}