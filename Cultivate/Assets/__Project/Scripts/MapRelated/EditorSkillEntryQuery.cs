
using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class EditorSkillEntryQuery
{
    [SerializeField] public string EntryName;
    [SerializeField] public WuXingPred WuXingPred = WuXingPred.任意;
    [SerializeField] public JingJieIndirect LowBaseJingJie = JingJieIndirect.练气;
    [SerializeField] public JingJieIndirect HighBaseJingJie = JingJieIndirect.返虚;
    [SerializeField] public EditorTag Tag;
}