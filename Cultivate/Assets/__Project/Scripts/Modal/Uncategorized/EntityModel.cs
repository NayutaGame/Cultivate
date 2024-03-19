
public interface EntityModel
{
    EntityEntry GetEntry();
    void SetEntry(EntityEntry entry);

    JingJie GetJingJie();
    void SetJingJie(JingJie jingJie);

    int GetSlotCount();
    void SetSlotCount(int slotCount);

    int GetBaseHealth();
    void SetBaseHealth(int health);

    int GetFinalHealth();

    bool IsNormal();
    void SetNormal(bool value);

    bool IsElite();
    void SetElite(bool value);

    bool IsBoss();
    void SetBoss(bool value);
}
