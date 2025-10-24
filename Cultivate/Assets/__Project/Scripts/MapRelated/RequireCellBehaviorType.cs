public enum RequireCellBehaviorType
{
    Consume,           // 耗材 - 有成功/失败两个分支
    RemoveFromPool,    // 移除 - 只有成功分支
    UpgradeJingJieToCurrent,    // 提升境界至当前境界 - 只有成功分支
    UpgradeJingJieToNext,       // 提升境界至下一境界 - 只有成功分支
    Copy,              // 复制 - 只有成功分支
    WuXingCycle,       // 五行相生 - 只有成功分支
}
