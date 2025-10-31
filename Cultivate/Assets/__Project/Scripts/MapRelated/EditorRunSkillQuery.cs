using System;
using CLLibrary;
using UnityEngine;

[Serializable]
public class EditorRunSkillQuery
{
	[SerializeField] public string EntryName = "";
	[SerializeField] public EditorWuXing WuXing = EditorWuXing.无;
	[SerializeField] public EditorJingJie JingJie = default;
	[SerializeField] public Bound BaseJingJieBound = global::JingJie.LianQi2HuaShen;
	[SerializeField] public EditorTag Tag = EditorTag.无;
	[SerializeField] public string Description = "请提交卡牌";
}