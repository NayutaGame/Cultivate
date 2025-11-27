
using System;
using UnityEngine;

[Serializable]
public class EditorRunSkillQuery
{
	[SerializeField] public string EntryName;
	[SerializeField] public EditorWuXing WuXing;
	[SerializeField] public EditorJingJie JingJie;
	[SerializeField] public EditorJingJie LowBaseJingJie;
	[SerializeField] public EditorJingJie HighBaseJingJie;
	[SerializeField] public EditorTag Tag;
	[SerializeField] public string Description = "请提交卡牌";
}