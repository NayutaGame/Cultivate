using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityModel
{
    EntityEntry GetEntry();
    void SetEntry(EntityEntry entry);
    JingJie GetJingJie();
    void SetJingJie(JingJie jingJie);
    int GetBaseHealth();
    void SetBaseHealth(int health);
    int GetFinalHealth();
    string ToJson();
    void FromJson(string json);
}
