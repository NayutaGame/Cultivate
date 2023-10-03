
public interface IEntityModel
{
    EntityEntry GetEntry();
    void SetEntry(EntityEntry entry);

    JingJie GetJingJie();
    void SetJingJie(JingJie jingJie);

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
