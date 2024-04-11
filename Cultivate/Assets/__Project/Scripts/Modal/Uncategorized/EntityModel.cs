
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

    int GetLadder();
    void SetLadder(int ladder);
}
