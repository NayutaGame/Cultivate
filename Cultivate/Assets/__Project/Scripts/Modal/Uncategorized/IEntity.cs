
using CLLibrary;

public interface IEntity
{
    EntityEntry GetEntry();
    void SetEntry(EntityEntry entry);

    JingJie GetJingJie();
    void SetJingJie(JingJie jingJie);

    int GetSlotCount();
    void SetSlotCount(int slotCount);

    int GetHealth();
    void SetHealth(int health);

    int GetLadder();
    void SetLadder(int ladder);

    Bound GetAllowedDifficulty();
    void SetAllowedDifficulty(Bound bound);

    bool IsInPool();
    void SetInPool(bool inPool);

    string GetReactionKeyFromSkill(RunSkill skill);
}
