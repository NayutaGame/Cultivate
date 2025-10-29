
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class EditorSkillEntryQuery
{
    [SerializeField] public string EntryName = "";
    [SerializeField] public WuXingType WuXing = WuXingType.Any;
    [SerializeField] public Bound BaseJingJieBound = JingJie.LianQi2HuaShen;
    [SerializeField] public TagType Tag = TagType.None;
}