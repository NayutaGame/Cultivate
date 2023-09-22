
using System;
using UnityEngine;

[Serializable]
public class EntityEditable
{
    // 不要Serializable, 记录Ref, 然后反序列化的时候重新链接
    private EntityEntry _entry;
    public EntityEntry GetEntry() => _entry;

    [SerializeField] private string EntityName;
    [SerializeField] private JingJie JingJie;
    [SerializeField] private int BaseHealth;
    [SerializeReference] public SlotListModel Slots;
    [SerializeField] private bool IsNormal;
    [SerializeField] private bool IsElite;
    [SerializeField] private bool IsBoss;
}
