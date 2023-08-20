
using System;
using System.Text;
using System.Threading.Tasks;

public class MingYuan : BoundedInt
{
    public MingYuan(int curr, int max) : base(curr, max) { }

    private static Tuple<int, int, int, int>[] MINGYUAN_PENALTY_TABLE = new[]
    {
        // hpPenalty, manaPenalty, powerPenalty, speedPenalty
        new Tuple<int, int, int, int>(80, 5, 5, 3),
        new Tuple<int, int, int, int>(40, 5, 5, 2),
        new Tuple<int, int, int, int>(40, 5, 3, 2),
        new Tuple<int, int, int, int>(40, 3, 3, 2),
        new Tuple<int, int, int, int>(20, 3, 3, 1),
        new Tuple<int, int, int, int>(20, 3, 1, 1),
        new Tuple<int, int, int, int>(20, 1, 1, 1),
        new Tuple<int, int, int, int>(10, 1, 1, 0),
        new Tuple<int, int, int, int>(10, 1, 0, 0),
        new Tuple<int, int, int, int>(10, 0, 0, 0),
    };

    public string GetMingYuanPenaltyText()
    {
        Tuple<int, int, int, int> penalty = MINGYUAN_PENALTY_TABLE[GetCurr()];
        if (GetCurr() >= 10)
            return "健康";

        StringBuilder sb = new();

        if (penalty.Item1 != 0) sb.Append($"损失{penalty.Item1}%生命上限");
        if (penalty.Item2 != 0) sb.Append($"遭受{penalty.Item2}灵气衰竭");
        if (penalty.Item3 != 0) sb.Append($"遭受{penalty.Item3}力量衰竭");
        if (penalty.Item4 != 0) sb.Append($"遭受{penalty.Item4}跳回合");

        return sb.ToString();
    }

    public async Task MingYuanPenaltyProcedure(StageEntity entity)
    {
        if (GetCurr() >= 10)
            return;

        Tuple<int, int, int, int> penalty = MINGYUAN_PENALTY_TABLE[GetCurr()];

        entity.MaxHp = (int)((float)entity.MaxHp * (100 - penalty.Item1) / 100);
        await entity.BuffSelfProcedure("灵气衰竭", penalty.Item2);
        await entity.BuffSelfProcedure("力量衰竭", penalty.Item3);
        await entity.BuffSelfProcedure("跳回合", penalty.Item4);
    }

    /*
     * 	            胜利命元	        胜利文本	                    失败命元            	        失败文本
     * 非Boss	    0	            胜利！                       -2                          你没能击败对手，损失了2命元。
     *                              获得了{xiuWeiValue}的修为                                 获得了{xiuWeiValue}修为
     *                              请选择一张卡作为奖励		                                请选择一张卡作为奖励
     *
     * 非化神      	3	            胜利！                       0	                        你没能击败对手，
     *                              跨越境界使得你的命元恢复了3                                 幸好跨越境界抵消了你的命元伤害。
     *                              获得了{xiuWeiValue}的修为                                 获得了{xiuWeiValue}修为
     *                              请选择一张卡作为奖励                                       请选择一张卡作为奖励
     *
     * default  	0	            你击败了强大的对手，           Set to 0	                你没能击败对手，
     *                              取得了最终的胜利！	                                        受到了致死的命元伤害。
     */
}
