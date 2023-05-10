using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityModel
{
    EntityEntry GetEntry();
    void SetEntry(EntityEntry entry);
    JingJie GetJingJie();
    void SetJingJie(JingJie jingJie);
    int GetHealth();
    void SetHealth(int health);
    string ToJson();
    void FromJson(string json);
}
