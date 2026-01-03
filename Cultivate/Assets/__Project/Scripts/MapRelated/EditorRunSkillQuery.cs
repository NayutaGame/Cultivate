
using System;
using UnityEngine;

[Serializable]
public class EditorRunSkillQuery
{
	[SerializeField] public string EntryName;
	[SerializeField] public WuXingPred WuXingPred = WuXingPred.任意;
	[SerializeField] public JingJiePred JingJiePred = JingJiePred.任意;
	[SerializeField] public JingJieIndirect LowBaseJingJie = JingJieIndirect.练气;
	[SerializeField] public JingJieIndirect HighBaseJingJie = JingJieIndirect.返虚;
	[SerializeField] public EditorTag Tag;
	[SerializeField] public string Description = "请提交卡牌";
	[SerializeField] public int AttackRequirement;
	[SerializeField] public int ArmorRequirement;
	[SerializeField] public int ManaRequirement;
	[SerializeField] public string BuffRequirement;
}