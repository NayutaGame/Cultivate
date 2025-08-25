
using CLLibrary;

public class MutatorPool
{
    private WeightedPool<SkillEntry>[] ItemPools;
    private WeightedPool<int>[] QualityPool;

    public MutatorPool()
    {
        ItemPools = new WeightedPool<SkillEntry>[5];
        for (int i = 0; i < ItemPools.Length; i++)
        {
            ItemPools[i] = new WeightedPool<SkillEntry>();
        }

        SkillEntry[] mutators = Encyclopedia.SkillCategory.Mutators;
        for (int i = 0; i < mutators.Length; i++)
        {
            SkillEntry mutator = mutators[i];
            int jingJie = mutator.GetJingJie();
            ItemPools[jingJie].Populate(1, mutator);
        }

        QualityPool = new WeightedPool<int>[5];
        for (int i = 0; i < QualityPool.Length; i++)
        {
            QualityPool[i] = new WeightedPool<int>();
        }

        QualityPool[0].Populate(100, 0);
        
        QualityPool[1].Populate(60, 0);
        QualityPool[1].Populate(35, 1);
        QualityPool[1].Populate(5, 2);
        
        QualityPool[2].Populate(30, 0);
        QualityPool[2].Populate(45, 1);
        QualityPool[2].Populate(20, 2);
        QualityPool[2].Populate(5, 3);
        
        QualityPool[3].Populate(15, 0);
        QualityPool[3].Populate(30, 1);
        QualityPool[3].Populate(35, 2);
        QualityPool[3].Populate(15, 3);
        QualityPool[3].Populate(5, 4);
        
        QualityPool[4].Populate(5, 0);
        QualityPool[4].Populate(15, 1);
        QualityPool[4].Populate(30, 2);
        QualityPool[4].Populate(35, 3);
        QualityPool[4].Populate(15, 4);
    }

    public bool Draw(out SkillEntry item, JingJie jingJie)
    {
        if (QualityPool[(int)jingJie].Draw(out int itemLevel))
        {
            if (ItemPools[itemLevel].Draw(out SkillEntry skillEntry))
            {
                item = skillEntry;
                return true;
            }
        }
        
        item = Encyclopedia.SkillCategory.Default();
        return false;
    }
}