using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class NodeCategory : Category<NodeEntry>
{
    public NodeCategory()
    {
        List = new()
        {
            new MarketNodeEntry("Market", "Market", null),

            new BattleNodeEntry("敌人", "敌人"),

            new AdventureNodeEntry("发现模板测试", "发现模板测试",
                create: runNode =>
                {
                    DiscoverSkillPanelDescriptor A = new();
                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("温泉", "温泉",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new("遇到温泉", "泡", "下潜", "离开");
                    DialogPanelDescriptor B = new("命元 + 2");
                    B._reward = new ResourceRewardDescriptor(mingYuan: 2);
                    DiscoverSkillPanelDescriptor C = new("随机水属当前境界卡牌1张", wuXing: WuXing.Shui);
                    DiscoverSkillPanelDescriptor D = new("随机火属当前境界卡牌1张", wuXing: WuXing.Huo);

                    A[0]._select = option => runNode.ChangePanel(B);
                    A[1]._select = option => runNode.ChangePanel(C);
                    A[2]._select = option => runNode.ChangePanel(D);

                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("集市地摊", "集市地摊",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new("在集市地摊上感应到模糊的灵气",
                        new DialogOption("5金币买下", new CostDetails(xiuWei: 5)),
                        new DialogOption("60金币整摊包了", new CostDetails(xiuWei: 60)),
                        "在此摆摊赚钱");
                    DialogPanelDescriptor B1 = new("判定失败");
                    DialogPanelDescriptor B2 = new("判定成功");
                    DiscoverSkillPanelDescriptor C = new("随机获得下品法宝", pred: s => s.SkillTypeCollection.Contains(SkillType.LingQi));
                    DialogPanelDescriptor D = new("获得35金币");
                    D._reward = new ResourceRewardDescriptor(xiuWei: 35);

                    A[0]._select = option => runNode.ChangePanel(RandomManager.value < 0.5f ? B1 : B2);
                    A[1]._select = option => runNode.ChangePanel(C);
                    A[2]._select = option => runNode.ChangePanel(D);

                    B2[0]._select = option => runNode.ChangePanel(C);

                    runNode.ChangePanel(A);
                }),

            new AdventureNodeEntry("镇上饭馆", "镇上饭馆",
                create: runNode =>
                {
                    DialogPanelDescriptor A1 = new("你路过镇上，前往饭馆打工挣钱，遇到后厨不同师傅之间闹矛盾",
                        "专心打工赚钱",
                        "帮烧火师傅说话",
                        "帮砍柴师傅说话",
                        "下页");
                    DialogPanelDescriptor A2 = new ("你路过镇上，前往饭馆打工挣钱，遇到后厨不同师傅之间闹矛盾",
                        "上页",
                        "帮挑水师傅说话",
                        "帮扫地师傅说话",
                        "帮切菜师傅说话");

                    DialogPanelDescriptor jinBi = new("获得50金币");
                    jinBi._reward = new ResourceRewardDescriptor(xiuWei: 50);

                    DiscoverSkillPanelDescriptor jin = new("获得金属卡牌1张", wuXing: WuXing.Jin);
                    DiscoverSkillPanelDescriptor shui = new("获得水属卡牌1张", wuXing: WuXing.Shui);
                    DiscoverSkillPanelDescriptor mu = new("获得木属卡牌1张", wuXing: WuXing.Mu);
                    DiscoverSkillPanelDescriptor huo = new("获得火属卡牌1张", wuXing: WuXing.Huo);
                    DiscoverSkillPanelDescriptor tu = new("获得土属卡牌1张", wuXing: WuXing.Tu);

                    A1[0]._select = option => runNode.ChangePanel(jinBi);
                    A1[1]._select = option => runNode.ChangePanel(huo);
                    A1[2]._select = option => runNode.ChangePanel(mu);
                    A1[3]._select = option => runNode.ChangePanel(A2);
                    A2[0]._select = option => runNode.ChangePanel(A1);
                    A2[1]._select = option => runNode.ChangePanel(shui);
                    A2[2]._select = option => runNode.ChangePanel(tu);
                    A2[3]._select = option => runNode.ChangePanel(jin);

                    runNode.ChangePanel(A1);
                }),

            new AdventureNodeEntry("夜中山间泉", "夜中山间泉",
                create: runNode =>
                {
                    DialogPanelDescriptor A = new("夜晚你在山上准备灌些泉水，水流在夜间乌黑发亮，捧起发现这水本身就呈黑色，恍惚间有奇怪的声音响起，一颗黑皮球从上流滚落下来",
                        "捡起黑皮球",
                        "品尝一下黑色泉水",
                        "赶快逃离这个鬼地方");

                    DialogPanelDescriptor A0 = new("你捡起黑皮球，表面柔软，摸起来像一颗快烂了的水蜜桃，你切开它，红色的汁液流了一地，里面露出了一枚淡黄色的果子",
                        "用它修炼",
                        "吸收其中灵气");

                    DialogPanelDescriptor A00 = new("选择一张卡牌境界提升");

                    DialogPanelDescriptor A01 = new("命元 + 2");

                    DialogPanelDescriptor A1 = new("你的身体理解了黑水的本质，获得水属性金丹期卡牌1张");

                    DialogPanelDescriptor A2 = new ("你从梦中醒来，手里握着一个道具");

                    runNode.ChangePanel(A1);
                }),

            // new AdventureNodeEntry("狂吾师叔事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("你遇见了狂吾师叔，他问你狂剑的见解", "说出自己的见解(需要有狂剑牌)", "说自己不懂，请师叔赐教");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("获得了一张练气牌");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("获得了一张练气牌");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             bool success = RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill skill, jingJie: JingJie.LianQi);
            //             if (success)
            //                 RunManager.Instance.Battle.SkillInventory.AddSkill(skill);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             bool success = RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill skill, jingJie: JingJie.LianQi);
            //             if (success)
            //                 RunManager.Instance.Battle.SkillInventory.AddSkill(skill);
            //
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),

            // new AdventureNodeEntry("神殿事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("来到一处神殿", "我必凯旋", "我已膨胀");
            //         BattlePanelDescriptor B = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你无法再获得命元，所有牌获得二动");
            //         DialogPanelDescriptor D = new DialogPanelDescriptor("胜利");
            //         DialogPanelDescriptor E = new DialogPanelDescriptor("你没能击败对手，虽然损失了一些命元，但还是获得了奖励");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(D);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(E);
            //                 }
            //             }
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("土匪事件", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor("你经过一处村庄，村口的石碑上刻着“问剑村”，村子里的村民各个面色凝重，这时一位老者向你走来，他告诉你：村庄饱受土匪强盗侵扰。你决定：", "迅速离去", "拔刀相助");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("自从筑基成功，你就正式踏上修仙的道路，凡尘的准则与琐事都与你无关。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("修仙者走的就是一条逆天而行的路，常年累月的修行换来的力量就是为了能够以自己的意志行走于大地之上。“老伯你放心，今日，我定给‘问剑村’一个安宁。”说罢，你一个飞身往土匪的藏身处遁去。");
            //         BattlePanelDescriptor D = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));//先用了你声明过得关键字
            //         BattlePanelDescriptor E = new BattlePanelDescriptor("鶸", new CreateEntityDetails(RunManager.Instance.Map.JingJie));
            //         DialogPanelDescriptor F = new DialogPanelDescriptor("你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor G = new DialogPanelDescriptor("你没能击败对手，损失了一些命元");
            //         DialogPanelDescriptor H = new DialogPanelDescriptor("土匪老大落败后提出：他在问剑村找到一本功法秘籍。他用秘籍作为交换希望你能放他一命。你决定：","同意","直接杀死土匪老大");
            //         DialogPanelDescriptor I = new DialogPanelDescriptor("土匪老大带你来到洞窟深处，打开某个暗门后，你看见一个石台上放着一本秘籍，趁你被秘籍吸引了注意，他掏出一个护符握在手中，护符化作一道金光划过天际，你回头一看，土匪老大逃走了。你决定：","遵从交易，放过他","追杀土匪老大");
            //         DialogPanelDescriptor J = new DialogPanelDescriptor("你没有管逃跑的土匪老大，继续在藏宝处寻找，在又找到一些宝藏后，你离开洞窟，继续赶路去了。");
            //         DialogPanelDescriptor K = new DialogPanelDescriptor("你成功追上了土匪老大，砍下了他的头颅，由于追的太投入，你找不到返回藏宝处的路，只能提着土匪老大的头返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor L = new DialogPanelDescriptor("你拼命追赶可还是被土匪老大逃掉了,由于追的太投入，你找不到返回藏宝处的路，只能拿着秘籍离开。");
            //         DialogPanelDescriptor M = new DialogPanelDescriptor("杀死土匪老大后，你砍下了他的头颅，你决定：", "尝试在洞窟中寻找秘籍", "带着头颅返回问剑村");
            //         DialogPanelDescriptor N = new DialogPanelDescriptor("你在洞窟深处发现一道暗门，打开暗门后你发现了一本功法秘籍还有一些其他的宝藏。收集完宝藏后，你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //         DialogPanelDescriptor O = new DialogPanelDescriptor("你在洞窟中四处寻找，可是一无所获，还触发了一些陷阱，损失了一些命元，所幸没有大碍。你带着土匪老大的头颅返回了问剑村，村长奖励了你一些XXX");
            //
            //
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? B : C);
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //            runNode.ChangePanel(D);
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(E);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(G);
            //                 }
            //             }
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             if (signal is BattleResultSignal battleResultSignal)
            //             {
            //                 if (battleResultSignal.State == BattleResultSignal.BattleResultState.Win)
            //                 {
            //                     runNode.ChangePanel(H);
            //                 }
            //                 else if (battleResultSignal.State == BattleResultSignal.BattleResultState.Lose)
            //                 {
            //                     runNode.ChangePanel(G);
            //                 }
            //             }
            //         };
            //
            //         F._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         G._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         H._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? I : M);
            //         };
            //
            //         I._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? J : K/*or L need judge*/);
            //         };
            //
            //         J._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         K._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         L._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         M._receiveSignal = (signal) =>
            //         {
            //             SelectedOptionSignal selectedOptionSignal = signal as SelectedOptionSignal;
            //             if (selectedOptionSignal == null)
            //                 return;
            //
            //             runNode.ChangePanel(selectedOptionSignal.Selected == 0 ? N/*or O need judge*/ : F);
            //         };
            //
            //         N._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         O._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),

            //
            // new AdventureNodeEntry("池塘", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor(
            //             "你经过一滩清池，一座亭子立于池中，你不禁驻足。墨绿色的亭柱顶着朱红色的宝顶，绕池一周，你并未看到通向亭子的道路，你决定：",
            //             "饮一口池水（回血）",
            //             "在池边停下修炼（回血药）",
            //             "尝试飞身跃入亭中（拥有一张筑基牌，则抽一张牌）。那亭子一看便不是凡物，里面说不好有一番机缘，你决定尝试飞身跃入亭中一探究竟。");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("池水清冽，你沿路奔波的劳累一扫而光。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你不禁暗想：这块宝地许是某位修士前辈开辟的宝地，不妨在这修炼一段时间，肯定大有裨益。");
            //         DialogPanelDescriptor D = new DialogPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");
            //         DialogPanelDescriptor E = new DialogPanelDescriptor("你纵身一跃，结果距离亭子仍有数尺，你落入水中，等你浮上水面时亭子已经消失了，你大感可惜，悻悻离去。");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 if (selectedOptionSignal.Selected == 0)
            //                 {
            //                     runNode.ChangePanel(B);
            //                 }
            //                 else if (selectedOptionSignal.Selected == 1)
            //                 {
            //                     runNode.ChangePanel(C);
            //                 }
            //                 else
            //                 {
            //                     bool flag = RunManager.Instance.AcquiredInventory.FirstObj(acquired =>
            //                                     acquired.GetJingJie() >= JingJie.ZhuJi) != null ||
            //                                 RunManager.Instance.Hero.HeroSlotInventory.Traversal.FirstObj(slot =>
            //                                     slot.GetJingJie().HasValue && slot.GetJingJie() >= JingJie.ZhuJi) !=
            //                                 null;
            //                     if (flag)
            //                     {
            //                         runNode.ChangePanel(D);
            //                     }
            //                     else
            //                     {
            //                         runNode.ChangePanel(E);
            //                     }
            //                 }
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         D._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         E._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
            //
            // new AdventureNodeEntry("路途劳累", "",
            //     create: runNode =>
            //     {
            //         DialogPanelDescriptor A = new DialogPanelDescriptor(
            //             "旅行数日，即便你已踏上修炼之道，路途上的劳累也无法完全消除（【惩罚】生命值惩罚5），你前往一处聚落决定休息几日，凝练修为。路过坊市时，一位清新脱俗的姑娘映入你的眼帘，",
            //             "不禁入神，驻足停留（【惩罚】【条件 驻足停留】 修为减少）",
            //             "加快脚步前往客栈（【奖励】回复生命值10，修为增加）",
            //             "前往坊市转转（跳转至商店界面）");
            //         DialogPanelDescriptor B = new DialogPanelDescriptor("看到她，你想起了江南的雨，你楞在原地，看出了神，直到背后的人推了你一把，你才回过神来。你暗暗一惊：你的道心不稳。修仙长路漫漫，需要长年累月的苦修，男女之情是修仙路上的一大阻碍，跟何况对方还是一位没有踏上仙路的女子。你赶快加快脚步离开此地。");
            //         DialogPanelDescriptor C = new DialogPanelDescriptor("你来到客栈后，就开始运转功法，一段时间后，旅途的劳累一扫而空，并巩固了修为。");
            //         // DialogPanelDescriptor D = new MarketPanelDescriptor("你纵深一跃，在空中一个翻身，稳稳落入亭中，亭子正中的石桌上，有一本《XXX》。");
            //
            //         A._receiveSignal = (signal) =>
            //         {
            //             if (signal is SelectedOptionSignal selectedOptionSignal)
            //             {
            //                 if (selectedOptionSignal.Selected == 0)
            //                 {
            //                     runNode.ChangePanel(B);
            //                 }
            //                 else if (selectedOptionSignal.Selected == 1)
            //                 {
            //                     runNode.ChangePanel(C);
            //                 }
            //                 else
            //                 {
            //                 }
            //             }
            //         };
            //
            //         B._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         C._receiveSignal = (signal) =>
            //         {
            //             RunManager.Instance.TryDrawAcquired(JingJie.LianQi);
            //             RunManager.Instance.Map.TryFinishNode();
            //         };
            //
            //         runNode.ChangePanel(A);
            //     }),
        };
    }
}
